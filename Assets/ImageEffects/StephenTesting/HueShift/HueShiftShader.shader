Shader "Hidden/HueShiftShader"
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

			float3 hueAdjust(float3 color, float hueAdjust)
			{
				const float3  kRGBToYPrime = float3(0.299, 0.587, 0.114);
				const float3  kRGBToI = float3(0.596, -0.275, -0.321);
				const float3  kRGBToQ = float3(0.212, -0.523, 0.311);

				const float3  kYIQToR = float3(1.0, 0.956, 0.621);
				const float3  kYIQToG = float3(1.0, -0.272, -0.647);
				const float3  kYIQToB = float3(1.0, -1.107, 1.704);

				// Convert to YIQ
				float   YPrime = dot(color, kRGBToYPrime);
				float   I = dot(color, kRGBToI);
				float   Q = dot(color, kRGBToQ);

				// Calculate the hue and chroma
				float   hue = atan2(Q, I);
				float   chroma = sqrt(I * I + Q * Q);

				// Make the user's adjustments
				hue += hueAdjust;

				// Convert back to YIQ
				Q = chroma * sin(hue);
				I = chroma * cos(hue);

				// Convert back to RGB
				float3    yIQ = float3(YPrime, I, Q);
				color.r = dot(yIQ, kYIQToR);
				color.g = dot(yIQ, kYIQToG);
				color.b = dot(yIQ, kYIQToB);

				// Save the result
				return color;
			}
			
			sampler2D _MainTex;
			float uHue;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 source = tex2D(_MainTex, i.uv);

				source.a = 1.0f;

				// Hue
				//if(source.rgb != vec3(1.0, 1.0, 1.0)) //Figure out how to make this less cancerous later
				//for some reason if the fragment is white this fucks up and makes it black
				if (source.r < 0.99 || source.g < 0.99 || source.b < 0.99)
				{
					source.rgb = hueAdjust(source.rgb, uHue);
				}

				return source;


			}
			ENDCG
		}
	}
}
