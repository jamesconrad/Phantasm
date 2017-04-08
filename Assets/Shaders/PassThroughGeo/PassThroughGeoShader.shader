Shader "Unlit/PassThroughGeoShader"
{
	Properties
	{
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input {
			float nothing;
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = 1.0;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
