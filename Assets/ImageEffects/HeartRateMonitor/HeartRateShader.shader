Shader "Hidden/HeartRateShader"
{
	Properties
	{
		_MainTex ("The Bump Texture", 2D) = "white" {}
		_HeartRateTex ("The Ramp Texture", 2D) = "white" {}
		_GridTex ("The Grid Texture", 2D) = "black" {}
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
			sampler2D _HeartRateTex;
			sampler2D _GridTex;
			float4 _Color;
			float4 _UVMult;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv * _UVMult) * _Color;
				fixed4 scrollHeart = tex2D(_HeartRateTex, (i.uv) - (_Time.rr * 15.0f));
				col = col * scrollHeart;
				scrollHeart =  tex2D(_GridTex, i.uv) * _Color * 0.5f;
				col.rgb += scrollHeart.rgb;
				col.a = 1.0f;
				return col;
			}
			ENDCG
		}
	}
}
