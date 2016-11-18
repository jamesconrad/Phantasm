// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/SplitModelUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (0.4,0.8,1.0,0.0)
		uWorldSpaceX("World Space X", Range(0,1)) = 0.0
		uWorldSpaceY("World Space Y", Range(0,1)) = 1.0
		uWorldSpaceZ("World Space Z", Range(0,1)) = 0.0
		uParam1("Parameter 1", Range(0,10)) = 5.0
		uParam2("Parameter 2", Range(0,1)) = 0.5
		//_OffsetUV("UV Offset", Vector) = (1.0, 1.0, 0.0, 0.0)
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		LOD 100
		Cull Front
		ZWrite Off
		//ZTest Off	
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float uWorldSpaceX;
			float uWorldSpaceY;
			float uWorldSpaceZ;
			float3 uOffset;
			float uParam1;
			float uParam2;
			float uAlpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				clip(frac(((i.worldPos.x + uOffset.x) * uWorldSpaceX + (i.worldPos.y + uOffset.y) * uWorldSpaceY + (i.worldPos.z + uOffset.z) * uWorldSpaceZ) * uParam1) - uParam2);

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;

				col.a = uAlpha;

				return col;
			}
			ENDCG
		}
	}
}
