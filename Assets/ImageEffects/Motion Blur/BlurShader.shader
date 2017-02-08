Shader "Hidden/BlurShader"
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
			float4 _BlurParam;

#define _BlurDirection _BlurParam.xy
#define _BlurAmount _BlurParam.w

			float _Amount;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 aspectRatio = float2(_ScreenParams.x / _ScreenParams.y, 1.0f);

				//_Location.xy /= _ScreenParams.xy;
				//float2 rippleUV = tex2D(_RippleTex, (i.uv - _Location.xy) * aspectRatio + 0.5f).xy;
				//rippleUV = rippleUV + rippleUV - 1.0f;

				fixed4 col = tex2D(_MainTex, i.uv);
				col += tex2D(_MainTex, saturate(i.uv + (aspectRatio * _BlurDirection.xy * _BlurAmount		)));
				col += tex2D(_MainTex, saturate(i.uv + (aspectRatio * _BlurDirection.xy * _BlurAmount * 2.0f)));
				col += tex2D(_MainTex, saturate(i.uv + (aspectRatio * _BlurDirection.xy * _BlurAmount		)));
				col += tex2D(_MainTex, saturate(i.uv + (aspectRatio * _BlurDirection.xy * _BlurAmount * 2.0f)));
				col *= 0.2f;
				
				return col;
			}
			ENDCG
		}
	}
}
