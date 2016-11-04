Shader "Custom/BrighterEmissiveThermalShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmissionTex("Emissive (RGB)", 2D) = "black" {}
		//_EmissionVisionMult("Emissive Mult", Range(0,10)) = 0.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _EmissionTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _EmissionVisionMult = 0.0f;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			float3 Emissive = tex2D(_EmissionTex, IN.uv_MainTex);// *_EmissionMult;
			o.Emission = Emissive + (float(Emissive.r + Emissive.g + Emissive.b).rrr * 0.333333333 * _EmissionVisionMult);
			//o.Emission = 
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
