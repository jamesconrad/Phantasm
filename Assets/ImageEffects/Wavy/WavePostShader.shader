Shader "Hidden/WavePost"
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
			uniform float2 uWaveCount = float2(4.0, 4.0);
			uniform float2 uWaveIntensity = float2(0.03, 0.03);
			uniform float2 uTime = float2(0.0, 0.0);

			//void main(void)
			//{
			//	col = texture(_MainTex, i.uv.xy + float(sin(i.uv.y * uWaveCount.x + uTime.x) * uWaveIntensity.x, sin(i.uv.x * uWaveCount.y + uTime.y) * uWaveIntensity.y));
			//}

			fixed4 frag (v2f i) : SV_Target
			{

				fixed4 col = tex2D(_MainTex, i.uv.xy + float2(sin(i.uv.y * uWaveCount.x + uTime.x) * uWaveIntensity.x, sin(i.uv.x * uWaveCount.y + uTime.y) * uWaveIntensity.y));

				return col;
			}
			ENDCG
		}
	}
}
