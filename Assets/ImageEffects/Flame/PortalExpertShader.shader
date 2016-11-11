Shader "Unlit/PortalExpertShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		uRGB("uRGB (RGB)", Color) = (0.4, 0.0, 0.4) 
		uUVMult("uUVMult", Vector) = (5.0, 5.0, 0.0, 0.0)
		uLoopAdd("uLoopAdd", Vector) = (0.3, 0.3, 0.0, 1.0)
		uLoopMult("uLoopMult", Vector) = (1.0, 10.0, 0.0, 1.0)
		uLoopFinalAdd("uLoopFinalAdd", Vector) = (4.0, -4.0, 0.0, 1.0)
		uFinalColAdd("uColAdd (RGB)", Color) = (0.2, 0.0, 0.2)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float4 uRGB = float4(0.333, 0.500, -0.150, 1.0f);
			float4 uColAdd = float4(0.1, 0.0, -0.05, 1.0f);
			float2 uUVMult = float2(5.0, 5.0);
			float4 uLoopAdd = float4(0.3, 0.3, 0.0, 1.0f);
			float4 uLoopMult = float4(1.0, 10.0, 0.0, 1.0f);
			float4 uLoopFinalAdd = float4(4.0, -4.0, 0.0, 1.0f);
			float4 uFinalColAdd = float4(0.1, 0.0, -0.05, 1.0f);
			
			fixed4 frag (v2f i) : SV_Target
			{
				float timeAdjust = -_Time * 1000.0f;
				float2 p = i.uv.xy * uUVMult.xy;
				for (int i = 1; i < 4; i++)
				{
					float2 newp = p;
					newp.x += (1.15f / float(i) * sin(float(i) * p.y + (timeAdjust *  0.3f) / 20.0f + uLoopAdd.x * float(i) * uLoopMult.x) + uLoopFinalAdd.x);
					newp.y += (1.55f / float(i) * cos(float(i) * p.x + (timeAdjust * -0.2f) / 20.0f + uLoopAdd.y * float(i) * uLoopMult.y) + uLoopFinalAdd.y);
					p = newp;
				}
				half3 col = float3(
					(uRGB.r * sin(1.0f * p.x) + uFinalColAdd.r),
					(uRGB.g * sin(1.5f * p.x) + uFinalColAdd.g),
					(uRGB.b * cos(1.0f * p.y) + uFinalColAdd.b));

					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);

					return half4(col, 1.0f);
			}
			ENDCG
		}
	}
}
