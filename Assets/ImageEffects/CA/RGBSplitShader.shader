Shader "Hidden/CAShader"
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
			
			float2 Pack(float2 param)
			{
				return param * 0.5 + 0.5;
			}

			float2 Unpack(float2 param)
			{
				return param + param - 1.0;
			}

			float rand(float2 co)
			{
				return frac(sin(1000.0*dot(co.xy, float2(21.5739, 43.421))) * 617284.3);
			}

			float rand(float co)
			{
				return frac(sin(1000.0 * co) * 617284.3);
			}

			uniform float PixelSizeX;
			uniform float PixelSizeY;
			uniform float uRandom = 0.0;
			uniform float uRange = 1.0;

			sampler2D _MainTex;
			
			float2 _Aspect = float2(1.0f, 1.0f);
			float _Dispersal = 1.00;
			uniform float _rOffsetX = -0.050;
			uniform float _rOffsetY = -0.050;
			uniform float _gOffsetX = 0.000;
			uniform float _gOffsetY = 0.000;
			uniform float _bOffsetX = 0.050; 
			uniform float _bOffsetY = 0.050;

			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// just invert the colors
				//col = 1 - col;
				//return col;

				float2 PixelSize = float2(PixelSizeX, PixelSizeY);
				float2 _rOffset = float2(_rOffsetX, _rOffsetY);
				float2 _gOffset = float2(_gOffsetX, _gOffsetY);
				float2 _bOffset = float2(_bOffsetX, _bOffsetY);

				//float4 rValue = tex2D(_MainTex, i.uv + (2.0f * PixelSize));
				//float4 gValue = tex2D(_MainTex, i.uv);
				//float4 bValue = tex2D(_MainTex, i.uv - (2.0f * PixelSize));

				//float4 rValue = tex2D(_MainTex, i.uv + 5.0 * PixelSize);
				//float4 gValue = tex2D(_MainTex, i.uv + 0.0 * PixelSize);
				//float4 bValue = tex2D(_MainTex, i.uv - 5.0 * PixelSize);

				// Combine the offset colors.
				//fixed4 col = float4(rValue.r, gValue.g, bValue.b, 1.0);

				//return col;

				float random = ((1.0 - uRange) + uRange * 2.0 * rand(float2(i.uv.y, uRandom)));

				float2 rTexOffset = i.uv + _rOffset * float2(random, random);
				float2 gTexOffset = i.uv + _gOffset * float2(random, random);
				float2 bTexOffset = i.uv + _bOffset * float2(random, random);

				float4 rValue = tex2D(_MainTex, rTexOffset);
				float4 gValue = tex2D(_MainTex, gTexOffset);
				float4 bValue = tex2D(_MainTex, bTexOffset);


				rValue += tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.250)) * 0.8;
				rValue += tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.500)) * 0.6;
				rValue += tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.750)) * 0.4;
				rValue += tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 1.000)) * 0.2;

				bValue += tex2D(_MainTex, lerp(bTexOffset, gTexOffset, 0.250)) * 0.8;
				bValue += tex2D(_MainTex, lerp(bTexOffset, gTexOffset, 0.500)) * 0.6;
				bValue += tex2D(_MainTex, lerp(bTexOffset, gTexOffset, 0.750)) * 0.4;
				bValue += tex2D(_MainTex, lerp(bTexOffset, gTexOffset, 1.000)) * 0.2;

				gValue += tex2D(_MainTex, lerp(gTexOffset, rTexOffset, 0.250)) * 0.75;
				gValue += tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.250)) * 0.75;
				gValue += tex2D(_MainTex, lerp(gTexOffset, rTexOffset, 0.500)) * 0.50;
				gValue += tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.500)) * 0.50;
				gValue += tex2D(_MainTex, lerp(gTexOffset, rTexOffset, 0.750)) * 0.25;
				gValue += tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.750)) * 0.25;

				//rValue *= 0.2;
				gValue *= 0.25;
				//bValue *= 0.2;
				rValue *= 0.33333333333333333;
				//gValue *= 0.33333333333333333;
				bValue *= 0.33333333333333333;

				fixed4 col = float4(rValue.r, gValue.g, bValue.b, 1.0);

				return col;

			}
			ENDCG
		}
	}
}
