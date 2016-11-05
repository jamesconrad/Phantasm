Shader "Hidden/TAAShader"
{
	Properties
	{
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

			struct Output
			{
				half4 first : SV_Target0;
				half4 second : SV_Target1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv.xy;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _PreviousTex;
			sampler2D _AccumulationTexture;
			sampler2D _CameraDepthTexture;
			sampler2D_half _CameraMotionVectorsTexture;

			float2 pixSize;

			uniform float4x4 _PreviousProjMat;
			float4x4 _VPMatrix;


			uniform float4 blendWeights;

			half2 GetClosestFragment(float2 uv);


			Output frag(v2f i) : SV_Target
			{
				half2 motion = tex2D(_CameraMotionVectorsTexture, i.uv).xy;
				float motionLen = length(motion);
											
				half4 col = tex2D(_MainTex, i.uv.xy);
				half4 colTL = tex2D(_MainTex, i.uv.xy + half2(-pixSize.x, pixSize.y));
				half4 colTR = tex2D(_MainTex, i.uv.xy + pixSize);
				half4 colBL = tex2D(_MainTex, i.uv.xy - pixSize);
				half4 colBR = tex2D(_MainTex, i.uv.xy + half2(pixSize.x, -pixSize.y));
			
				float4 minimum = min(colTL, min(colTR, min(colBL, colBR)));
				float4 maximum = max(colTL, max(colTR, max(colBL, colBR)));
			
				half4 colPrev = tex2D(_PreviousTex, i.uv.xy - motion);
				colPrev = clamp(colPrev, minimum, maximum);
			
				half blendW = clamp(lerp(blendWeights.x, blendWeights.y, motionLen * blendWeights.z), blendWeights.y, blendWeights.x);
			
				col = lerp(col, colPrev, blendW);
				Output finalOut;
				finalOut.first = col;
				finalOut.second = col;
				return finalOut;
			}



			half2 GetClosestFragment(float2 uv)
			{
				const float4 neighborhood = float4(
				tex2D(_CameraDepthTexture, uv - pixSize).r,
				tex2D(_CameraDepthTexture, uv + float2(pixSize.x, -pixSize.y)).r,
				tex2D(_CameraDepthTexture, uv + float2(-pixSize.x, pixSize.y)).r,
				tex2D(_CameraDepthTexture, uv + pixSize).r);

				float3 result = float3(0., 0., tex2D(_CameraDepthTexture, uv).r);


				if (neighborhood.x < result.z)
					result = float3(-1., -1., neighborhood.x);

				if (neighborhood.y < result.z)
					result = float3(1., -1., neighborhood.y);

				if (neighborhood.z < result.z)
					result = float3(-1., 1., neighborhood.z);

				if (neighborhood.w < result.z)
					result = float3(1., 1., neighborhood.w);

				return (uv + result.xy);
			}


			ENDCG
		}
	}
}

//You should check this out when you have the opportunity to do so
//// Tonemapper from http://gpuopen.com/optimized-reversible-tonemapper-for-resolve/
//float getMaximumElement(in float3 value)
//{
//	// AMD_shader_trinary_minmax
//	return max(max(value.x, value.y), value.z);
//}