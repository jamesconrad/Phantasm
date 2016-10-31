Shader "Hidden/SSAOShader"
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
			
			float3 Pack(float3 param)
			{
				return param * 0.5 + 0.5;
			}

			float3 Unpack(float3 param)
			{
				return param + param - 1.0;
			}

			//inline void DecodeDepthNormal(float4 enc, out float depth, out float3 normal)
			//{
			//	depth = DecodeFloatRG(enc.zw);
			//	normal = DecodeViewNormalStereo(enc);
			//}

			sampler2D _MainTex;
			sampler2D _CameraDepthNormalsTexture;

			float4x4 uProjBiasMatrixInverse;

			float3 normalValues;
			float depthValue;

			fixed4 frag (v2f i) : SV_Target
			{
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv.xy), depthValue, normalValues);
				fixed4 col = fixed4(Pack(normalValues), 1.0f);

				float4 fragPos = mul(uProjBiasMatrixInverse, float4(i.uv.xy, depth, 1.0f));
				fragPos.xyz /= fragPos.w;



				//fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col.rgb = tex2D(_CameraDepthTexture, i.uv).rrr;
				return col;
			}
			ENDCG
		}
	}
}
