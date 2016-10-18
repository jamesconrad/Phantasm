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
				return param;// *0.5 + 0.5;
			}

			float2 Unpack(float2 param)
			{
				return param;// +param - 1.0;
			}

			sampler2D _MainTex;
			
			

			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// just invert the colors
				//col = 1 - col;
				//return col;
			
				float2 _Aspect = float2(1.0f, 1.0f);
				float _Dispersal = 1.00;
				float _rOffset = -0.050;
				float _gOffset = 0.000;
				float _bOffset = 0.050;

				float2 TexOffset = Unpack(i.uv);

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

				rTexOffset = rTexOffset / 20.0f - 0.5;
				gTexOffset = gTexOffset / 20.0f - 0.5;
				bTexOffset = bTexOffset / 20.0f - 0.5;
				//rTexOffset = Pack(rTexOffset);
				//gTexOffset = Pack(gTexOffset);
				//bTexOffset = Pack(bTexOffset);

				rValue = tex2D(_MainTex, rTexOffset + i.uv);
				gValue = tex2D(_MainTex, gTexOffset + i.uv);
				bValue = tex2D(_MainTex, bTexOffset + i.uv);

				// Combine the offset colors.
				fixed4 col = float4(rValue.r, gValue.g, bValue.b, 1.0);

				return col;




			}
			ENDCG
		}
	}
}
