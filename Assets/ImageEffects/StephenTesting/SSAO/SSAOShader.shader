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

			//const float3 RAND_SAMPLES[SAMPLE_COUNT] = {
			//	float3(0.01305719,0.5872321,-0.119337),
			//	float3(0.3230782,0.02207272,-0.4188725),
			//	float3(-0.310725,-0.191367,0.05613686),
			//	float3(-0.4796457,0.09398766,-0.5802653),
			//	float3(0.1399992,-0.3357702,0.5596789),
			//	float3(-0.2484578,0.2555322,0.3489439),
			//	float3(0.1871898,-0.702764,-0.2317479),
			//	float3(0.8849149,0.2842076,0.368524),

				

			//inline void DecodeDepthNormal(float4 enc, out float depth, out float3 normal)
			//{
			//	depth = DecodeFloatRG(enc.zw);
			//	normal = DecodeViewNormalStereo(enc);
			//}

			sampler2D _MainTex;
			sampler2D _CameraDepthNormalsTexture;

			float uRadius = 0.0625;

			float4x4 uProjBiasMatrixInverse;
			float4x4 uProj;

			float3 normal;
			float depth;

			const int kernelSize = 16;
			

			half3 frag (v2f i) : SV_Target
			{
			//	const float3  samples[] = {
			//	float3(0.14840700, -0.133314 0.1506750),
			//	float3(0.16187000, -0.128777 0.1455610),
			//	float3(0.06263370, -0.132009 0.2171340),
			//	float3(0.02966160, -0.252805 0.1076520),
			//	float3(0.03951750, -0.262670 0.1325810),
			//	float3(0.19147700, 0.1764530 0.1915370),
			//	float3(-0.1612670, 0.2202880 0.2276490),
			//	float3(0.24657500, 0.2411850 0.1895140),
			//	float3(-0.0103453, -0.275986 0.3393100),
			//	float3(-0.1822100, 0.2695660 0.3627670),
			//	float3(-0.0833288, -0.527435 0.0984072),
			//	float3(0.18961900, 0.5640350 0.1063950),
			//	float3(0.43003300, 0.3541680 0.3755700),
			//	float3(0.51916100, 0.2713890 0.4604560),
			//	float3(0.76124700, -0.302437 0.0914969),
			//	float3(0.33867700, 0.8104540 0.2346690),
			//};

			float3  samples[16];

				samples[ 0] = float3(0.14840700, -0.133314, 0.1506750);
				samples[ 1] = float3(0.16187000, -0.128777, 0.1455610);
				samples[ 2] = float3(0.06263370, -0.132009, 0.2171340);
				samples[ 3] = float3(0.02966160, -0.252805, 0.1076520);
				samples[ 4] = float3(0.03951750, -0.262670, 0.1325810);
				samples[ 5] = float3(0.19147700, 0.1764530, 0.1915370);
				samples[ 6] = float3(-0.1612670, 0.2202880, 0.2276490);
				samples[ 7] = float3(0.24657500, 0.2411850, 0.1895140);
				samples[ 8] = float3(-0.0103453, -0.275986, 0.3393100);
				samples[ 9] = float3(-0.1822100, 0.2695660, 0.3627670);
				samples[10] = float3(-0.0833288, -0.527435, 0.0984072);
				samples[11] = float3(0.18961900, 0.5640350, 0.1063950);
				samples[12] = float3(0.43003300, 0.3541680, 0.3755700);
				samples[13] = float3(0.51916100, 0.2713890, 0.4604560);
				samples[14] = float3(0.76124700, -0.302437, 0.0914969);
				samples[15] = float3(0.33867700, 0.8104540, 0.2346690);

				//Need to get actual random vectors later
				float3 randomVec = float3(1.0f, 0.0f, 0.0f);


				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv.xy), depth, normal);
				fixed4 col;// = fixed4(Pack(normal), 1.0f);
				
				float3 tangent = normalize(randomVec - (normal * dot(randomVec, normal)));
				float3 bitangent = cross(normal, tangent);
				float3x3 TBN = float3x3(tangent, bitangent, normal);
				float4 fragPos = mul(uProjBiasMatrixInverse, float4(i.uv.xy, depth, 1.0f));
				fragPos.xyz /= fragPos.w;

				float2 texcoord = i.uv.xy;

				float occlusion = 0.0;
				for (int i = 0; i < kernelSize; ++i)
				{
					// get sample position
					float3 samplePos = mul(TBN, samples[i]); // From tangent to view-space
																  //samplePos = fragPos.xyz + samplePos * uRadius; 
					samplePos = fragPos.xyz + samplePos * (uRadius*depth);

					// project sample position (to sample texture) (to get position on screen/texture)
					float4 offset = float4(samplePos, 1.0);
					offset = mul(uProj, offset); // from view to clip-space
					offset.xy /= offset.w; // perspective divide
					offset.xy = offset.xy * 0.5 + 0.5; // transform to range 0.0 - 1.0

																  // get sample depth
					float sampleDepth;// = tex2D(uDepthMap, offset.xy).r;
					DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, offset.xy), sampleDepth, normal);

					//float4 sampleDepthToPos = uProjBiasMatrixInverse * float4(i.uv.xy, sampleDepth, 1.0);
					float4 sampleDepthToPos = mul(uProjBiasMatrixInverse, float4(texcoord, sampleDepth, 1.0f));
					sampleDepthToPos /= sampleDepthToPos.w;

					float rangeCheck = 1.0;
					// range check & accumulate		
					rangeCheck = abs(samplePos.z - sampleDepthToPos.z) < uRadius ? 1.0 : 0.0;
					occlusion += (sampleDepthToPos.z > samplePos.z ? 1.0 : 0.0) * rangeCheck;
				}
				occlusion = 1.0 - ((occlusion * 1.25 * (1.5 - depth)) / kernelSize);

				col.rgb = occlusion.rrr;
				col.a = 1.0f;

				//fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col.rgb = tex2D(_CameraDepthTexture, i.uv).rrr;
				return col;
			}
			ENDCG
		}
	}
}
