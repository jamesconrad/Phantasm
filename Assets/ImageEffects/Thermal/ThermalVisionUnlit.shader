Shader "Unlit/ThermalVisionUnlit"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ThermalRamp("Thermal (RGB)", 2D) = "grey" {}
		_Temperature("Temperature", Range(0,1)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					// include file that contains UnityObjectToWorldNormal helper function
			#include "UnityCG.cginc"
		
			struct v2f {
				// we'll output world space normal as one of regular ("texcoord") interpolators
				half3 worldNormal : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
		
			// vertex shader: takes object space normal as input too
			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				// UnityCG.cginc file contains function to transform
				// normal from object to world space, use that
				//o.worldNormal = UnityObjectToWorldDir(normal);
				float3 viewN = normalize(mul(UNITY_MATRIX_IT_MV, normal.xyzz).xyz);
				o.worldNormal = viewN;
				return o;
			}		

			sampler2D _ThermalRamp;
			half _Temperature;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = 0;
				//UNITY_MATRIX_IT_MV[2].xyz
				float dotProduct = (dot(float3(0.0f, 0.0f, 1.0f), i.worldNormal) * _Temperature); //
				float4 thermalRamp = tex2D(_ThermalRamp, float2(max(dotProduct, 0.05f) * 0.925f, 0.0f));

				//c.rgb = i.worldNormal*0.5 + 0.5;
				c = thermalRamp;
				//c.r = dotProduct;
				//c.g = dotProduct;
				//c.b = dotProduct;
				//c.rgb = viewN * 0.5 + 0.5;
				return c;
			}
			ENDCG
		}
	}
}
