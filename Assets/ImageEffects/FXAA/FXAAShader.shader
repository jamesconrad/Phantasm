Shader "Hidden/FXAAShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			#define FXAA_SPAN_MAX 8.0
			#define FXAA_REDUCE_MIN 0.078125
			#define FXAA_REDUCE_MUL 0.125
			#define FXAA_EDGE_THRESHOLD 0.125
			#define FXAA_EDGE_THRESHOLD_MIN 0.0625
			#define FXAA_SUBPIX_TRIM 0.25
			#define FXAA_SUBPIX_TRIM_SCALE 1.0
			#define FXAA_SUBPIX_CAP 0.75

			
			sampler2D_half _MainTex;

			float2 invVP;
			float2 blurDir;

			half4 frag (v2f i) : SV_Target
			{
				//fixed4 blurFrag = 1 - (1 - tex2D(Blur1Tex, i.uv)) * (1 - tex2D(Blur2Tex, i.uv)) * (1 - tex2D(Blur3Tex, i.uv)) * (1 - tex2D(Blur4Tex, i.uv));
				//half4 blurFrag = tex2D(Blur1Tex, i.uv) + tex2D(Blur2Tex, i.uv) + tex2D(Blur3Tex, i.uv) + tex2D(Blur4Tex, i.uv);
				invVP = 1 / _ScreenParams.xy;

				half3 l = fixed3(0.299, 0.587, 0.114);
				half3 lN = tex2D(_MainTex, i.uv + float2(1.0, 1.0) * invVP).rgb;
				half3 lW = tex2D(_MainTex, i.uv + float2(-1.0, 1.0) * invVP).rgb;
				half3 lM = tex2D(_MainTex, i.uv).rgb;
				half3 lE = tex2D(_MainTex, i.uv + float2(1.0, -1.0) * invVP).rgb;
				half3 lS = tex2D(_MainTex, i.uv + float2(-1.0, -1.0) * invVP).rgb;

				blurDir.x = -((lS + lE) - (lN + lW));
				blurDir.y = ((lS + lW) - (lE + lN));

				float dirReduce = max((lS + lE + lN + lW) * (0.25 * FXAA_REDUCE_MUL), FXAA_REDUCE_MIN);
				float blurScale = 1.0 / (min(abs(blurDir.x), abs(blurDir.y)) + dirReduce);

				blurDir = min(float2(FXAA_SPAN_MAX, FXAA_SPAN_MAX),
					max(float2(-FXAA_SPAN_MAX, -FXAA_SPAN_MAX),
						blurDir * blurScale)) * invVP;

				float3 res1 = (0.5 * (tex2D(_MainTex, i.uv + (blurDir * (1.0 / 3.0 - 0.5))).xyz + tex2D(_MainTex, i.uv + (blurDir * (2.0 / 3.0 - 0.5))).xyz));
				float3 res2 = (tex2D(_MainTex, i.uv + (blurDir * (0.0 / 3.0 - 0.5))).xyz + tex2D(_MainTex, i.uv + (blurDir * (3.0 / 3.0 - 0.5))).xyz);
				float3 result = res1 * 0.5 + res2 * 0.25;

				//if the new luma is too far off, then we use res1
				float luma = dot(l, result);
				float lMin = min(lM, min(min(lS, lE), min(lW, lN)));
				float lMax = max(lM, max(max(lS, lE), max(lW, lN)));
				float lRange = lMax - lMin;
				if (luma < lRange)
					return half4(res1 /*+ blurFrag.rgb*/, 1.0); //outside of range, use the actual value
				else
					return half4(result /*+ blurFrag.rgb*/, 1.0);

				
				
			}
			ENDCG
		}
	}
}
