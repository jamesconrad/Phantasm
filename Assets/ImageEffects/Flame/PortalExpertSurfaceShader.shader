Shader "Custom/PortalExpertSurfaceShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		uRGB("uRGB (RGB)", Color) = (0.4, 0.0, 0.4) 
		uUVMult("uUVMult", Vector) = (5.0, 5.0, 0.0, 0.0)
		uLoopAdd("uLoopAdd", Vector) = (0.3, 0.3, 0.0, 1.0)
		uLoopMult("uLoopMult", Vector) = (1.0, 10.0, 0.0, 1.0)
		uLoopFinalAdd("uLoopFinalAdd", Vector) = (4.0, -4.0, 0.0, 1.0)
		uFinalColAdd("uColAdd (RGB)", Color) = (0.2, 0.0, 0.2)
		//_Color ("Color", Color) = (1,1,1,1)
		//_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
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
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float4 uRGB = float4(0.333, 0.500, -0.150, 1.0f);
		float4 uColAdd = float4(0.1, 0.0, -0.05, 1.0f);
		float2 uUVMult = float2(5.0, 5.0);
		float4 uLoopAdd = float4(0.3, 0.3, 0.0, 1.0f);
		float4 uLoopMult = float4(1.0, 10.0, 0.0, 1.0f);
		float4 uLoopFinalAdd = float4(4.0, -4.0, 0.0, 1.0f);
		float4 uFinalColAdd = float4(0.1, 0.0, -0.05, 1.0f);

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float timeAdjust = -_Time.x * 1000.0f;
			float2 p = IN.uv_MainTex.xy * uUVMult.xy;
			for (int ii = 1; ii < 4; ii++)
			{
				float2 newp = p;
				newp.x += (1.15f / float(ii) * sin(float(ii) * p.y + (timeAdjust *  0.3f) / 20.0f + uLoopAdd.x * float(ii) * uLoopMult.x) + uLoopFinalAdd.x);
				newp.y += (1.55f / float(ii) * cos(float(ii) * p.x + (timeAdjust * -0.2f) / 20.0f + uLoopAdd.y * float(ii) * uLoopMult.y) + uLoopFinalAdd.y);
				p = newp;
			}
			half3 col = float3(
				(uRGB.r * sin(1.0f * p.x) + uFinalColAdd.r),
				(uRGB.g * sin(1.5f * p.x) + uFinalColAdd.g),
				(uRGB.b * cos(1.0f * p.y) + uFinalColAdd.b));
			
			o.Emission =  col;



			//// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo = c.rgb;
			//// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = 1.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
