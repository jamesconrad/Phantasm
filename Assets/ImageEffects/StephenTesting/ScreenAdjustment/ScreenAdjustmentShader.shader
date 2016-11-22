Shader "Hidden/ScreenAdjustment"
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
			uniform float4 uParam;
			uniform float4 uInputOutput;


			#define uBrightness uParam.x
			#define uContrast uParam.y
			#define uGamma uParam.z
			#define uSomething2 uParam.w
			
			#define uMinInput uInputOutput.x
			#define uMaxInput uInputOutput.y
			#define uMinOutput uInputOutput.z
			#define uMaxOutput uInputOutput.w


			float3 BrightnessContrast(float3 color)
			{
				return color * uContrast + uBrightness;
			}

			float3 LevelAdjustment(float3 color)
			{
				color = min(
					max(color - uMinInput.rrr, 0.0f) 
					/ (uMaxInput.rrr - uMinInput.rrr
						), 1.0f);

				color = pow(color, uGamma.rrr);
				color = lerp(uMinOutput.rrr, uMaxOutput.rrr, color);

				return color;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float3 col = tex2D(_MainTex, i.uv);
				col = LevelAdjustment(col);
				col = BrightnessContrast(col);

				return fixed4(col, 1.0f);
			}
			ENDCG
		}
	}
}
