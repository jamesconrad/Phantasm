Shader "Unlit/GoochUnlitShader"
{
	Properties
	{		
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex("Ramp (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 0.8, 0.2, 1.0)
		_DiffuseScale("Diffuse Scale", Float) = 1.0
		_DiffuseBias("Diffuse Bias", Float) = 0.0
		_DiffuseExponent("Diffuse Exponent", float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
			//#pragma vertex vert
			//#pragma fragment frag
			//// make fog work
			//#pragma multi_compile_fog
			#pragma surface surf NPR
			
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

			//_LightColor0 is default unity shit
			//It's really hard to follow what exists and doesn't
			half4 LightingNPR(SurfaceOutput o, half3 lightdir, half3 viewdir, half atten)
			{
				float lambert = saturate(dot(o.Normal, lightdir));
				half4 diffuse = half4(_LightColor0.rgb * atten * o.Albedo.rgb * lambert, 1.0);
				diffuse *= tex2D(_RampTex, float2(lambert, 0.0));

				return diffuse;
			}

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
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
