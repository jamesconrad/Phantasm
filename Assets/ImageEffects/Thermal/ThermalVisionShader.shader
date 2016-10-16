Shader "Custom/ThermalVisionShader" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_Amount("Extrusion Amount", Range(-1,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		//#pragma surface surf Lambert vertex:vert
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 customColor;
		};
		
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//v.vertex.xyz += v.normal * _Amount;
			o.customColor = abs(v.normal);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			o.Albedo = IN.customColor;
			o.Albedo = float3(1.0f, 1.0f, 1.0f);

			//o.Albedo = c.rgb;
			//// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			//o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
