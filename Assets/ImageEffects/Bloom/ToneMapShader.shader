Shader "Hidden/ToneMapShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		GammaA("GammaA", float) = 0.0001
		GammaY("GammaY", float) = 0.5
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
			
			sampler2D_half _MainTex;

			float GammaA;
			float GammaY;

			half4 frag (v2f i) : SV_Target
			{
				half4 col;
				//col.rgb = 2.0 * tex2D(_MainTex, i.uv).rgb - 1.0;
				col.rgb = tex2D(_MainTex, i.uv).rgb;
				half l = (col.r + col.g + col.b) / 3.0;
				l = GammaA * pow(l, GammaY);
				col.rgb = (half3(l, l, l)/* * 2 - 1) * 2 - 1*/) * col.rgb;
				col.a = 1;
				return saturate(col);
			}
			ENDCG
		}
	}
}
