Shader "Hidden/SobelShader"
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

			//sampler2D uNormalMap;
			//sampler2D uDepthMap;

			float2 uPixelSize = 1.0f; 
				
			float edgeThresholdNormal = 6.0; //5.0
			float edgeThresholdDepth = 0.24; //0.0010
			
			float Sobel_Normal_Horizontal(sampler2D texmap, float2 texcoord);
			float Sobel_Normal_Vertical(sampler2D texmap, float2 texcoord);
			float Sobel_Depth_Horizontal(sampler2D texmap, float2 texcoord);
			float Sobel_Depth_Vertical(sampler2D texmap, float2 texcoord);
			
			float Sobel_Depth_NESW(sampler2D texmap, float2 texcoord);
			float Sobel_Depth_NWSE(sampler2D texmap, float2 texcoord);
						
			float Sobel_Depth_NESW_thicc(sampler2D texmap, float2 texcoord);
			float Sobel_Depth_NWSE_thicc(sampler2D texmap, float2 texcoord);
			
			float Sobel_Normal_Horizontal(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float3 sum = 0.0f;
				
				//Left column
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y + pixelSize.y	)).rgb;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y				)).rgb * 2.0;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y - pixelSize.y	)).rgb;
				
				//Right column
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y + pixelSize.y	)).rgb;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y				)).rgb * 2.0;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y - pixelSize.y	)).rgb;
				
				//sqrt(x * x + y * y + z * z) = length(vector)
				//x * x + y * y + z * z		= dot(sum, sum)
				float edgeFactor = dot(sum, sum);
				
				if(edgeFactor < edgeThresholdNormal)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}
			
			float Sobel_Normal_Vertical(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float3 sum = 0.0f;
				
				//Top column
				sum += tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y + pixelSize.y)).rgb;
				sum += tex2D(texmap, float2(texcoord.x				, texcoord.y + pixelSize.y)).rgb * 2.0;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y + pixelSize.y)).rgb;
				
				//Bottom column
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y - pixelSize.y)).rgb;
				sum -= tex2D(texmap, float2(texcoord.x				, texcoord.y - pixelSize.y)).rgb * 2.0;
				sum -= tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y - pixelSize.y)).rgb;
				
				//sqrt(x * x + y * y + z * z) = length(vector)
				//x * x + y * y + z * z		= dot(sum, sum)
				float edgeFactor = dot(sum, sum);
				
				if(edgeFactor < edgeThresholdNormal)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}
			
			float Sobel_Depth_Horizontal(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float sum = 0.0;
				
				//Left column
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y + pixelSize.y	)).r;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y					)).r * 2.0;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y - pixelSize.y	)).r;
				
				//Right column
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y + pixelSize.y	)).r;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y					)).r * 2.0;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y - pixelSize.y	)).r;
				

				if(abs(sum) < edgeThresholdDepth)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}
			
			float Sobel_Depth_Vertical(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float sum = 0.0;
				
				//Top column
				sum += tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y + pixelSize.y)).r;
				sum += tex2D(texmap, float2(texcoord.x				, texcoord.y + pixelSize.y)).r * 2.0;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y + pixelSize.y)).r;
				
				//Bottom column
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y - pixelSize.y)).r;
				sum -= tex2D(texmap, float2(texcoord.x				, texcoord.y - pixelSize.y)).r * 2.0;
				sum -= tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y - pixelSize.y)).r;
				
				if(abs(sum) < edgeThresholdDepth)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}




			float Sobel_Depth_NWSE(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float sum = 0.0;
				
				//NW
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y + pixelSize.y	)).r * 2.0;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y				)).r;
				sum -= tex2D(texmap, float2(texcoord.x				, texcoord.y + pixelSize.y	)).r;
				
				//SE
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y - pixelSize.y	)).r * 2.0;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y				)).r;
				sum += tex2D(texmap, float2(texcoord.x				, texcoord.y - pixelSize.y	)).r;
							

				if(abs(sum) < edgeThresholdDepth)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}

			float Sobel_Depth_NESW(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float sum = 0.0;
				
				//NW
				sum -= tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y + pixelSize.y	)).r * 2.0;
				sum -= tex2D(texmap, float2(texcoord.x + pixelSize.x, texcoord.y				)).r;
				sum -= tex2D(texmap, float2(texcoord.x				, texcoord.y + pixelSize.y	)).r;
				
				//SE
				sum += tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y - pixelSize.y	)).r * 2.0;
				sum += tex2D(texmap, float2(texcoord.x - pixelSize.x, texcoord.y				)).r;
				sum += tex2D(texmap, float2(texcoord.x				, texcoord.y - pixelSize.y	)).r;
							

				if(abs(sum) < edgeThresholdDepth)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}

			//float Sobel_Depth_NWSE_thicc(sampler2D texmap, float2 texcoord, float2 pixelSize)
			//{
			//	float sum = 0.0;
			//	
			//	float2 pixelSize2 = pixelSize + pixelSize;
			//
			//	//NW
			//	sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x,  texcoord.y + pixelSize.y	)).r;
			//	sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x,  texcoord.y + pixelSize2.y)).r;
			//	sum -= tex2D(texmap, float2(texcoord.x - pixelSize2.x, texcoord.y + pixelSize.y	)).r;
			//	sum -= tex2D(texmap, float2(texcoord.x - pixelSize2.x, texcoord.y + pixelSize2.y)).r;
			//	
			//	//SE				
			//	sum += tex2D(texmap, float2(texcoord.x + pixelSize.x,  texcoord.y - pixelSize.y	)).r;
			//	sum += tex2D(texmap, float2(texcoord.x + pixelSize.x,  texcoord.y - pixelSize2.y)).r;
			//	sum += tex2D(texmap, float2(texcoord.x + pixelSize2.x, texcoord.y - pixelSize.y	)).r;
			//	sum += tex2D(texmap, float2(texcoord.x + pixelSize2.x, texcoord.y - pixelSize2.y)).r;
			//				
			//
			//	if(abs(sum) < edgeThresholdDepth)
			//	{
			//		return 1.0;
			//	}
			//	else
			//	{
			//		return 0.0;
			//	}
			//}

			float Sobel_Depth_NWSE_thicc(sampler2D texmap, float2 texcoord, float2 pixelSize)
			{
				float sum = 0.0;
				
				float2 pixelSize2 = pixelSize + pixelSize;
				float2 pixelSize3 = pixelSize2 + pixelSize;

				//NW
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize.x,  texcoord.y + pixelSize.y	)).r;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize2.x, texcoord.y + pixelSize2.y)).r * 2.0f;
				sum -= tex2D(texmap, float2(texcoord.x - pixelSize3.x, texcoord.y + pixelSize3.y)).r;
				
				//SE				
				sum += tex2D(texmap, float2(texcoord.x + pixelSize.x,  texcoord.y - pixelSize.y	)).r;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize2.x, texcoord.y - pixelSize2.y)).r * 2.0f;
				sum += tex2D(texmap, float2(texcoord.x + pixelSize3.x, texcoord.y - pixelSize3.y)).r;
							

				if(abs(sum) < edgeThresholdDepth)
				{
					return 1.0;
				}
				else
				{
					return 0.0;
				}
			}




			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;


			fixed4 frag (v2f i) : SV_Target
			{
				uPixelSize = 1.0f / _ScreenParams.xy;

				edgeThresholdNormal = 6.0; //5.0
				edgeThresholdDepth = 0.010; //0.0010

				fixed4 col = tex2D(_MainTex, i.uv);


				float depthSobel = Sobel_Depth_Horizontal(_CameraDepthTexture, i.uv, uPixelSize) * Sobel_Depth_Vertical(_CameraDepthTexture, i.uv, uPixelSize);
				//float normalSobel = Sobel_Normal_Horizontal(uNormalMap, i.uv) * Sobel_Normal_Vertical(uNormalMap, i.uv);
				
				depthSobel = Sobel_Depth_NWSE_thicc(_CameraDepthTexture, i.uv, uPixelSize) ;
				col = col * depthSobel;// * normalSobel;
				//col = depthSobel;

				return col;
			}
			ENDCG
		}
	}
}
