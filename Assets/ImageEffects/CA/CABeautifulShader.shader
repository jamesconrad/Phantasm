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

			sampler2D _MainTex;

			float _Dispersal;
			float _Offset;
			

			fixed4 frag (v2f i) : SV_Target
			{			
				float2 _Aspect = float2(1.0f, 1.0f);
				float _rOffset = -_Offset;
				float _gOffset = 0.000;
				float _bOffset = _Offset;

				float2 TexOffset = Unpack(i.uv.xy);

				float2 rTexOffset = TexOffset;
				float2 gTexOffset = TexOffset;
				float2 bTexOffset = TexOffset;

				float2 absTexOffset = abs(TexOffset);
				float2 isPositive = absTexOffset / TexOffset;

				if ((TexOffset.x) != 0.0)
				{
					float distanceToCenter = absTexOffset.x * _Aspect.x;
					float distanceToCenterSquared = pow(distanceToCenter, _Dispersal);

					rTexOffset.x += _rOffset * distanceToCenterSquared * isPositive.x;
					gTexOffset.x += _gOffset * distanceToCenterSquared * isPositive.x;
					bTexOffset.x += _bOffset * distanceToCenterSquared * isPositive.x;
				}

				if ((TexOffset.y) != 0.0)
				{
					float distanceToCenter = absTexOffset.y * _Aspect.y;
					float distanceToCenterSquared = pow(distanceToCenter, _Dispersal);

					rTexOffset.y += _rOffset * distanceToCenterSquared * isPositive.y;
					gTexOffset.y += _gOffset * distanceToCenterSquared * isPositive.y;
					bTexOffset.y += _bOffset * distanceToCenterSquared * isPositive.y;
				}

				float4 rValue;// = float4(0.0f, 0.0f, 0.0f, 0.0f);
				float4 gValue;// = float4(0.0f, 0.0f, 0.0f, 0.0f);
				float4 bValue;// = float4(0.0f, 0.0f, 0.0f, 0.0f);

				//rTexOffset = rTexOffset;
				//gTexOffset = gTexOffset;
				//bTexOffset = bTexOffset;
				rTexOffset = Pack(rTexOffset);
				gTexOffset = Pack(gTexOffset);
				bTexOffset = Pack(bTexOffset);


				rValue = tex2D(_MainTex, rTexOffset);
				gValue = tex2D(_MainTex, gTexOffset);
				bValue = tex2D(_MainTex, bTexOffset);

				float4 sceneR16 = tex2D(_MainTex, rTexOffset);
				float4 sceneR15 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.0625));
				float4 sceneR14 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.1250));
				float4 sceneR13 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.1875));
				float4 sceneR12 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.2500));
				float4 sceneR11 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.3125));
				float4 sceneR10 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.3750));
				float4 sceneR09 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.4375));
				float4 sceneR08 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.5000));
				float4 sceneR07 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.5625));
				float4 sceneR06 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.6250));
				float4 sceneR05 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.6875));
				float4 sceneR04 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.7500));
				float4 sceneR03 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.8125));
				float4 sceneR02 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.8750));
				float4 sceneR01 = tex2D(_MainTex, lerp(rTexOffset, gTexOffset, 0.9375));
				float4 sceneG00 = tex2D(_MainTex, gTexOffset);
				float4 sceneB01 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.0625));
				float4 sceneB02 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.1250));
				float4 sceneB03 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.1875));
				float4 sceneB04 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.2500));
				float4 sceneB05 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.3125));
				float4 sceneB06 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.3750));
				float4 sceneB07 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.4375));
				float4 sceneB08 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.5000));
				float4 sceneB09 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.5625));
				float4 sceneB10 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.6250));
				float4 sceneB11 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.6875));
				float4 sceneB12 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.7500));
				float4 sceneB13 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.8125));
				float4 sceneB14 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.8750));
				float4 sceneB15 = tex2D(_MainTex, lerp(gTexOffset, bTexOffset, 0.9375));
				float4 sceneB16 = tex2D(_MainTex, bTexOffset);





				rValue += sceneR16 * 0.111;
				rValue += sceneR15 * 0.222;
				rValue += sceneR14 * 0.333;
				rValue += sceneR13 * 0.444;
				rValue += sceneR12 * 0.555;
				rValue += sceneR11 * 0.666;
				rValue += sceneR10 * 0.777;
				rValue += sceneR09 * 0.888;
				rValue += sceneR08;
				rValue += sceneR07 * 0.888;
				rValue += sceneR06 * 0.777;
				rValue += sceneR05 * 0.666;
				rValue += sceneR04 * 0.555;
				rValue += sceneR03 * 0.444;
				rValue += sceneR02 * 0.333;
				rValue += sceneR01 * 0.222;
				rValue += sceneG00 * 0.111;

				bValue += sceneB16 * 0.111;
				bValue += sceneB15 * 0.222;
				bValue += sceneB14 * 0.333;
				bValue += sceneB13 * 0.444;
				bValue += sceneB12 * 0.555;
				bValue += sceneB11 * 0.666;
				bValue += sceneB10 * 0.777;
				bValue += sceneB09 * 0.888;
				bValue += sceneB08;
				bValue += sceneB07 * 0.888;
				bValue += sceneB06 * 0.777;
				bValue += sceneB05 * 0.666;
				bValue += sceneB04 * 0.555;
				bValue += sceneB03 * 0.444;
				bValue += sceneB02 * 0.333;
				bValue += sceneB01 * 0.222;
				bValue += sceneG00 * 0.111;

				gValue += sceneG00;
				gValue += sceneR01 * 0.888;
				gValue += sceneB01 * 0.888;
				gValue += sceneR02 * 0.777;
				gValue += sceneB02 * 0.777;
				gValue += sceneR03 * 0.666;
				gValue += sceneB03 * 0.666;
				gValue += sceneR04 * 0.555;
				gValue += sceneB04 * 0.555;
				gValue += sceneR05 * 0.444;
				gValue += sceneB05 * 0.444;
				gValue += sceneR06 * 0.333;
				gValue += sceneB06 * 0.333;
				gValue += sceneR07 * 0.222;
				gValue += sceneB07 * 0.222;
				gValue += sceneR08 * 0.111;
				gValue += sceneB08 * 0.111;

				rValue *= 0.111111111111;
				gValue *= 0.111111111111;
				bValue *= 0.111111111111;


				rValue *= 0.9;
				gValue *= 0.9;
				bValue *= 0.9;


				// Combine the offset colors.
				fixed4 col = float4(rValue.r, gValue.g, bValue.b, 1.0);

				return col;




			}
			ENDCG
		}
	}
}
