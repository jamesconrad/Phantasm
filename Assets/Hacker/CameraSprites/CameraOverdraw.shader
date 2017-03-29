Shader "Unlit/CameraOverdraw"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Blend One One 
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				//float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				//float2 uv : TEXCOORD0;
				float4 ref : TEXCOORD0;
			};
						
			//sampler2D _MainTex;
			//float4 _MainTex_ST;
			float4 uColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.ref = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//float interp = 0.0f;
				//if(i.ref.w < 0.1f)
				//	interp = 1.0f;
				//if(i.ref.w > -0.1f)
				//	interp = 1.0f;
				fixed4 col = uColor; //float4(0.2, 0.05, 0.025, 1.0);

				float timeAdjust = -_Time.r * 1000.0f; 
				float2 p = i.vertex.xy * 0.01f + i.vertex.zz * 0.5f;
				for (int i = 1; i < 4; i++)
				{
					float2 newp = p;
					newp.x += (1.15f / float(i) * sin(float(i) * p.y + (timeAdjust *  0.3f) / 20.0f + 0.3f * float(i)				) + 4.0f);
					newp.y += (1.55f / float(i) * cos(float(i) * p.x + (timeAdjust * -0.2f) / 20.0f + 0.3f * float(i) * 10.0f	) - 4.0f);
					p = newp;
				}

				col.a *= min(0.25f * sin(p.x) + 0.5f, 1.0f) + 0.0f;

				//col.rgb *= interp;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
