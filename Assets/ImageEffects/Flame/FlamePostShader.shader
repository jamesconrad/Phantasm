Shader "Hidden/FlamePostShader"
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
			
			float4 uRGB = float4(0.333, 0.500, -0.150, 1.0f);
			float4 uColAdd = float4(0.1, 0.0, -0.05, 1.0f);

			fixed4 frag (v2f i) : SV_Target
			{
				float timeAdjust = -_Time * 1000.0f; 
				float2 p = i.uv.xy * 5.0f;
				for (int i = 1; i < 4; i++)
				{
					float2 newp = p;
					newp.x += (1.15f / float(i) * sin(float(i) * p.y + (timeAdjust *  0.3f) / 20.0f + 0.3f * float(i)				) + 4.0f);
					newp.y += (1.55f / float(i) * cos(float(i) * p.x + (timeAdjust * -0.2f) / 20.0f + 0.3f * float(i) * 10.0f	) - 4.0f);
					p = newp;
				}
				half3 col = float3(
					(uRGB.r * sin(1.0f * p.x) + uColAdd.r), 
					(uRGB.g * sin(1.5f * p.x) + uColAdd.g), 
					(uRGB.b * cos(1.0f * p.y) + uColAdd.b));

				return half4(col, 1.0f);
				}
			ENDCG
		}
	}
}
