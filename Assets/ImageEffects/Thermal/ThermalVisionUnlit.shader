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
				half3 normal : TEXCOORD0;
				half3 viewDir : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
		
			// vertex shader: takes object space normal as input too
			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.viewDir = ObjSpaceViewDir(vertex);
				o.normal = normal;
				return o;
			}		

			sampler2D _ThermalRamp;
			half _Temperature;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = 0;
				//UNITY_MATRIX_IT_MV[2].xyz 
				//-normalize(-_WorldSpaceCameraPos + mul(i.pos.xyz, _Object2World))

				float3 normalDirection = normalize(i.normal);
				float3 viewDirection = normalize(i.viewDir);

				float dotProduct = saturate(dot(normalDirection, viewDirection)) * _Temperature; //
				float4 thermalRamp = tex2D(_ThermalRamp, float2(max(dotProduct, 0.05f) * 0.925f, 0.0f));

				//c.rgb = i.normal*0.5 + 0.5;
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
