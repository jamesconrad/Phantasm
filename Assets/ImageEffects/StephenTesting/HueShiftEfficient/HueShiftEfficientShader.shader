Shader "Hidden/HueShiftEfficientShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		uMapTex ("HueTable", 2D) = "white" {}
		uLookupTex ("Lookup", 2D) = "white" {}
		uOffset("Offset", float) = 0.0
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

			float3 RGBtoHSL(float3 RGB)
			{
				float H;
				float S;
				float L;
			
				float Min = min( RGB.r, min( RGB.g, RGB.b ));    //Min. value of RGB
				float Max = max( RGB.r, max( RGB.g, RGB.b ));    //Max. value of RGB
				float deltaMax = Max - Min ;            //Delta RGB value

				L = ( Max + Min ) / 2.0;

				if ( deltaMax == 0 )                     //This is a gray, no chroma...
				{
				   H = 0;                                //HSL results from 0 to 1
				   S = 0;
				}
				else                                    //Chromatic data...
				{
					if ( L < 0.5 ) S = deltaMax / ( Max + Min );
					else           S = deltaMax / ( 2.0 - Max - Min );

					float deltaR = ( ( ( Max - RGB.r ) / 6.0 ) + ( deltaMax / 2.0 ) ) / deltaMax;
					float deltaG = ( ( ( Max - RGB.g ) / 6.0 ) + ( deltaMax / 2.0 ) ) / deltaMax;
					float deltaB = ( ( ( Max - RGB.b ) / 6.0 ) + ( deltaMax / 2.0 ) ) / deltaMax;

					if      ( RGB.r == Max ) H = deltaB - deltaG;
					else if ( RGB.g == Max ) H = ( 1.0 / 3.0 ) + deltaR - deltaB;
					else if ( RGB.b == Max ) H = ( 2.0 / 3.0 ) + deltaG - deltaR;
				
					H = frac(H);
					//if ( H < 0 ) H += 1.0;
					//if ( H > 1 ) H -= 1.0;
				}
			
				return float3(H, S, L);
			}
			
			sampler2D _MainTex;
			sampler2D uMapTex;
			sampler2D uLookupTex;
			float uHue;
			float uOffset;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 source = tex2D(_MainTex, i.uv);

				source.a = 1.0f;

				float3 HSL = RGBtoHSL(source.rgb);

				float H = HSL.x;
				float L = HSL.z;

				//H = min(H, 1.0);
				//H = max(H, 0.0);

				float S =	tex2D(uLookupTex, float2(H + frac(uOffset), 0.0)).r * HSL.y;
				float3 MapColor = tex2D(uMapTex, float2(H, saturate(1.0-S))).rgb;

				return fixed4(MapColor, source.a);


			}
			ENDCG
		}
	}
}
