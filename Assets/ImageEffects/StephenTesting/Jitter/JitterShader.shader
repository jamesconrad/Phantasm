Shader "Hidden/JitterShader"
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

			float rand(float2 co)
			{
				return frac(sin(1000.0*dot(co.xy, float2(21.5739, 43.421))) * 617284.3);
			}			

			float rand(float co)
			{
				return frac(sin(1000.0 * co) * 617284.3);
			}
			
			sampler2D _MainTex;
			float4 param;

#define RandomNumber param.xy 
#define jitterAmount param.zw 

			//float2 jitterAmount;
			//float2 RandomNumber;



			fixed4 frag (v2f i) : SV_Target
			{
				float2 offset = float2(rand(RandomNumber.xx + i.uv) , rand(RandomNumber.yy + i.uv));
				offset = offset + offset - 1.0f;
				offset = offset * jitterAmount / _ScreenParams.xy;

				fixed4 col = tex2D(_MainTex, i.uv + offset);
				// just invert the colors
				
				return col;
			}
			ENDCG
		}
	}
}
