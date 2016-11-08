Shader "Custom/GoochShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex("Ramp (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 0.8, 0.2, 1.0)
		_DiffuseScale("Diffuse Scale", Float) = 1.0
		_DiffuseBias("Diffuse Bias", Float) = 0.0
		_DiffuseExponent("Diffuse Exponent", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and don't enable shadows on all light types
		#pragma surface surf NPR noambient

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		uniform sampler2D _RampTex;

		struct Input {
			float2 uv_MainTex;
		};

		//_LightColor0 is default unity shit
		//It's really hard to follow what exists and doesn't
		half4 LightingNPR(SurfaceOutput o, half3 lightdir, half3 viewdir, half atten)
		{
			float lambert = dot(o.Normal, lightdir);
			half4 diffuse = half4(_LightColor0.rgb * atten * o.Albedo.rgb * saturate(lambert), 1.0);
			diffuse *= tex2D(_RampTex, float2((lambert * 0.5 + 0.5), 0.0));

			return diffuse;
		}

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
