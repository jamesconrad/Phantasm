Shader "Custom/ThermalVisionShader" 
{
	Properties 
	{
		_MainTex("Texture", 2D) = "white" {}
		_ThermalRamp("Thermal (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
		}
			LOD 200
			
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows
			//#pragma surface surf Lambert vertex:vert
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _ThermalRamp;

			struct Input {
				float2 uv_MainTex;
				float3 customColor;
				float3 worldNormal;
		};
		
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//v.vertex.xyz += v.normal * _Amount;
			//o.customColor = abs(v.normal);
			float3 viewN = normalize(mul(UNITY_MATRIX_IT_MV, v.normal.xyzz).xyz);
			o.worldNormal = viewN;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			//o.Emission = tex2D(_MainTex, IN.uv_MainTex).rgb;
			//o.Emission = IN.customColor;
			//o.Emission = float3(1.0f, 1.0f, 1.0f);

			float dotProduct = dot(float3(0.0f, 0.0f, 1.0f), IN.worldNormal);
			dotProduct = dotProduct  * 0.5 + 0.5;
			float4 thermalRamp = tex2D(_ThermalRamp, float2(clamp(dotProduct, 0.0f, 1.0f) * 0.8f, 0.0f));
			o.Emission = thermalRamp;

			//o.Emission.r = dotProduct;
			//o.Emission.g = dotProduct;
			//o.Emission.b = dotProduct;

			//o.Emission = IN.worldNormal*0.5 + 0.5;

			o.Albedo = 0;
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
