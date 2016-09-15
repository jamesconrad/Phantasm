Shader "Hidden/HorizontalBlur"
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

			uniform float PixelSize;
			
			sampler2D_half _MainTex;

			half4 frag (v2f i) : SV_Target
			{
				half4 col;
				col.rgb  = tex2D(_MainTex, float2(i.uv.x - 4.0 * PixelSize, i.uv.y)).rgb * 0.06;
				col.rgb += tex2D(_MainTex, float2(i.uv.x - 3.0 * PixelSize, i.uv.y)).rgb * 0.09;
				col.rgb += tex2D(_MainTex, float2(i.uv.x - 2.0 * PixelSize, i.uv.y)).rgb * 0.12;
				col.rgb += tex2D(_MainTex, float2(i.uv.x -		 PixelSize, i.uv.y)).rgb * 0.15;
				col.rgb += tex2D(_MainTex, float2(i.uv.x,					i.uv.y)).rgb * 0.16;
				col.rgb += tex2D(_MainTex, float2(i.uv.x +		 PixelSize, i.uv.y)).rgb * 0.15;
				col.rgb += tex2D(_MainTex, float2(i.uv.x + 2.0 * PixelSize, i.uv.y)).rgb * 0.12;
				col.rgb += tex2D(_MainTex, float2(i.uv.x + 3.0 * PixelSize, i.uv.y)).rgb * 0.09;
				col.rgb += tex2D(_MainTex, float2(i.uv.x + 4.0 * PixelSize, i.uv.y)).rgb * 0.06;
				col.a = 1.0;
				return col;
			}
			ENDCG
		}
	}
}
