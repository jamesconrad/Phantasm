Shader "Custom/SplitModelShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_WorldSpaceX ("World Space X", Range(0,1)) = 0.0
		_WorldSpaceY ("World Space Y", Range(0,1)) = 1.0
		_WorldSpaceZ ("World Space Z", Range(0,1)) = 0.0
		_Param1("Parameter 1", Range(0,10)) = 5.0
		_Param2("Parameter 2", Range(0,1)) = 0.5

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

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _WorldSpaceX;
		float	_WorldSpaceY;
		float	_WorldSpaceZ;
		float _Param1;
		float _Param2;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			clip(frac((IN.worldPos.x * _WorldSpaceX + IN.worldPos.y * _WorldSpaceY + IN.worldPos.z * _WorldSpaceZ) * _Param1) - _Param2);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
