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


			fixed4 frag(v2f i) : SV_Target
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

				// Combine the offset colors.
				fixed4 col = float4(rValue.r, gValue.g, bValue.b, 1.0);

				return col;




			}
			ENDCG
		}
	}
}
