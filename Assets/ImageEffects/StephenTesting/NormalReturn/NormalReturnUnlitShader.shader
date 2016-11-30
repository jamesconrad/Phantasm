Shader "Unlit/ThermalVisionUnlit"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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
				half3 worldNormal : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
		
			// vertex shader: takes object space normal as input too
			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				// UnityCG.cginc file contains function to transform
				// normal from object to world space, use that
				float3 viewN = normalize(mul(UNITY_MATRIX_IT_MV, normal.xyzz).xyz);
				//o.worldNormal = UnityObjectToWorldDir(normal);
				o.worldNormal = viewN;
				return o;
			}
		
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = 0;
				c.rgb = i.worldNormal * 0.5 + 0.5;
				//c.rgb = viewN * 0.5 + 0.5;
				return c;
			}
			ENDCG
		}
	}
}
