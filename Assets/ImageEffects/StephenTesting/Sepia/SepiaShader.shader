Shader "Hidden/SepiaShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				float3 sepia;

				sepia.r = (col.r * 0.393) + (col.g * 0.769) + (col.b * 0.189);
				sepia.g = (col.r * 0.349) + (col.g * 0.686) + (col.b * 0.168);
				sepia.b = (col.r * 0.272) + (col.g * 0.534) + (col.b * 0.131);

				//outColor.rgb = mix(source.rgb, vec3(luminance), uAmount);
				col = float4(sepia.xyz, col.w);

				return col;
			}
			ENDCG
		}
	}
}
