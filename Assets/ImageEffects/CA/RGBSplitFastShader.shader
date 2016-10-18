Shader "Hidden/CAShader"
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
			
			float2 Pack(float2 param)
			{
				return param * 0.5 + 0.5;
			}

			float2 Unpack(float2 param)
			{
				return param + param - 1.0;
			}

			uniform float PixelSizeX;
			uniform float PixelSizeY;
			sampler2D _MainTex;
			
			float2 _Aspect = float2(1.0f, 1.0f);
			float _Dispersal = 1.00;
			uniform float2 _rOffset = -0.050;
			uniform float2 _gOffset = 0.000;
			uniform float2 _bOffset = 0.050;

			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// just invert the colors
				//col = 1 - col;
				//return col;

				float2 PixelSize = float2(PixelSizeX, PixelSizeY);

				float4 rValue = tex2D(_MainTex, i.uv + (2.0f * PixelSize));
				float4 gValue = tex2D(_MainTex, i.uv);
				float4 bValue = tex2D(_MainTex, i.uv - (2.0f * PixelSize));

				//float4 rValue = tex2D(_MainTex, i.uv + 5.0 * PixelSize);
				//float4 gValue = tex2D(_MainTex, i.uv + 0.0 * PixelSize);
				//float4 bValue = tex2D(_MainTex, i.uv - 5.0 * PixelSize);

				// Combine the offset colors.
				fixed4 col = float4(rValue.r, gValue.g, bValue.b, 1.0);

				return col;



			}
			ENDCG
		}
	}
}
