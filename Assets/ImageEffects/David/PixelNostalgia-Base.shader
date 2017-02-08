//////////////////////////////////////////////////////////////
//
//  Author: David F. Arppe - May 2016
//  ------------------
//
//  Simulates various effects to create a retro game feel.
//  multi_compile should yield 8 copies of this effect to
//  speed the shader up.
//
//////////////////////////////////////////////////////////////

Shader "Hidden/NostalgiaPixels-Base"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		[HideInInspector] _redBits("float", Float) = 255
		[HideInInspector] _greenBits("float", Float) = 255
		[HideInInspector] _blueBits("float", Float) = 255

		[HideInInspector] _xRes("float", Float) = 1024
		[HideInInspector] _yRes("float", Float) = 1024
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma multi_compile BITS_OFF BITS_ON
			#pragma multi_compile DITHER_OFF DITHER_ON
			#pragma multi_compile MONOCHROME_OFF MONOCHROME_ON

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

			float _redBits = 255;
			float _greenBits = 255;
			float _blueBits = 255;

			float _xRes;
			float _yRes;
						
			float FindClosest(int x, int y, float c0)
			{
				int dither[8][8] =
				{
					{ 0, 32, 8, 40, 2, 34, 10, 42 },
					{ 48, 16, 56, 24, 50, 18, 58, 26 },
					{ 12, 44, 4, 36, 14, 46, 6, 38 },
					{ 60, 28, 52, 20, 62, 30, 54, 22 },
					{ 3, 35, 11, 43, 1, 33, 9, 41 },
					{ 51, 19, 59, 27, 49, 17, 57, 25 },
					{ 15, 47, 7, 39, 13, 45, 5, 37 },
					{ 63, 31, 55, 23, 61, 29, 53, 21 }
				};

				float limit = ((float)dither[x][y] + 1.0f) / 64.0f;

				if (c0 < limit)
					return 0.0;
				return 1.0;
			}

			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 p = (i.uv * fixed2(_xRes, _yRes));
				fixed2 uv = fixed2(floor(p.x), floor(p.y)) / fixed2(_xRes, _yRes);
				fixed4 col = tex2D(_MainTex, uv);
#if defined(DITHER_ON)
				fixed4 dCol = col;
#endif

				float bInc = 1.0f / 256.0f;
				float rInc = bInc;
				float gInc = bInc;
#if defined(BITS_ON)
				rInc = 1.0f / (_redBits + 1.0f);
				gInc = 1.0f / (_greenBits + 1.0f);
				bInc = 1.0f / (_blueBits + 1.0f);

				float r = floor(col.r * _redBits) / _redBits;
				float g = floor(col.g * _greenBits) / _greenBits;
				float b = floor(col.b * _blueBits) / _blueBits;

				col = fixed4(r, g, b, col.a);
#endif

#if defined(DITHER_ON)
				fixed2 xy = i.uv * fixed2(_xRes, _yRes);
				int x = floor(fmod(xy.x, 8.0f));
				int y = floor(fmod(xy.y, 8.0f));

				fixed4 finalRGB;
				finalRGB.r = col.r + (rInc * FindClosest(x, y, dCol.r));
				finalRGB.g = col.g + (gInc * FindClosest(x, y, dCol.g));
				finalRGB.b = col.b + (bInc * FindClosest(x, y, dCol.b));
				finalRGB.a = 1.0;

#if defined(MONOCHROME_ON)
				fixed4 lum = fixed4(0.299, 0.587, 0.114, 0.0);
				float d = dot(lum, finalRGB);
				finalRGB = fixed4(d, d, d, 1.0f);
#endif

				return finalRGB;
#else

#if defined(MONOCHROME_ON)
				fixed4 lum = fixed4(0.299, 0.587, 0.114, 0.0);
				float d = dot(lum, col);
				col = fixed4(d, d, d, 1.0f);
#endif

				return col;
#endif
			}
			ENDCG
		}
	}
}
