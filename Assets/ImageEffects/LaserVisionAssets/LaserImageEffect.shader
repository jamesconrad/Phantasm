Shader "Hidden/LaserImageEffect"
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
			
			sampler2D _MainTex;			
			sampler2D _Tex;
			float timeMult;
			float alphaAmount;
			float alphaAdd;
			float4 ColorMult;
			float uDistance;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_Tex, i.uv);

				float timeAdjust = -_Time * timeMult * 1.0f; 
				float2 p = i.uv.xy * 10.0f * float2(1.0f * uDistance, 1.0f);
				for (int i = 1; i < 4; i++)
				{
					float2 newp = p;
					newp.x += (1.15f / float(i) * sin(float(i) * p.y + (timeAdjust *  0.3f) / 20.0f + 0.3f * float(i)				) + 4.0f);
					newp.y += (1.55f / float(i) * cos(float(i) * p.x + (timeAdjust * -0.2f) / 20.0f + 0.3f * float(i) * 10.0f	) - 4.0f);
					p = newp;
				}

				col.rgb *= min(alphaAmount * sin(p.x) + 0.8f, 1.0f) + alphaAdd;
				col.rgb *= col.a;
				return col * ColorMult;
			}
			ENDCG
		}
	}
}
