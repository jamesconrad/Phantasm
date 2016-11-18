Shader "Unlit/GhostUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0.4,0.4,0.4,0.0)
		_RimPower("Rim Power", Range(0.25,8.0)) = 3.0
		_ExtrusionAdd("Extrusion Time Amount", Range(0,1.0)) = 0.0
		_Extrusion("Extrusion Amount", Range(0,1.0)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent"}
		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
				half randomRim : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD3;
				//float3 viewDirection;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			half _Extrusion;
			half _ExtrusionAdd;
			
			v2f vert (appdata v, float3 normal : NORMAL)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
				//o.viewDirection = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				o.normal = normal;
				o.randomRim = sin((v.vertex.x + v.vertex.y + v.vertex.z) * 50.0f + _ExtrusionAdd * 1.0f) * 0.3 + 1.0;
				o.vertex.xyz += normal * sin((v.vertex.x + v.vertex.y + v.vertex.z) * 50.0f + _ExtrusionAdd * 1.0f) * _Extrusion;
				//float3 viewN = normalize(mul(UNITY_MATRIX_IT_MV, normal.xyzz).xyz);
				//o.worldNormal = viewN;
				return o;
			}
			
			
			float4 _RimColor;
			float _RimPower;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				float3 normalDirection = normalize(i.normal);
				float3 viewDirection = normalize(i.viewDir);

				half rim = 1.0 - saturate(dot(viewDirection, normalDirection));
				//half rimAmount = pow(rim, _RimPower * i.randomRim.r);
				half rimAmount = pow(rim, _RimPower * i.randomRim.r);
				col.a = rimAmount;
				col.rgb *= _RimColor.rgb;

				return col;
			}
			ENDCG
		}
	}
}
