Shader "Unlit/GhostUnlitWavyExpertShader"
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
		Tags 
		{ 
			"RenderType"="Transparent" 
			"IgnoreProjector"="True" 
			"Queue"="Transparent"
		}
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
				float2 screenuv : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float3 objectPos : TEXCOORD3;
				float4 vertex : SV_POSITION;
				float depth : DEPTH;
				float3 normal : NORMAL;
				half randomRim : TEXCOORD4;
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
				//o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				
				o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w;

				o.screenuv = ((o.vertex.xy / o.vertex.w) + 1)/2;
				o.screenuv.y = 1 - o.screenuv.y;

				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
				//o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//o.normal = UnityObjectToWorldNormal(v.normal);
				o.normal = normal;
								
				o.objectPos = v.vertex.xyz;	

				o.randomRim = sin((v.vertex.x + v.vertex.y + v.vertex.z) * 50.0f + _ExtrusionAdd * 1.0f) * 0.3 + 1.0;
				o.vertex.xyz += normal * sin((v.vertex.x + v.vertex.y + v.vertex.z) * 50.0f + _ExtrusionAdd * 1.0f) * _Extrusion;
				//float3 viewN = normalize(mul(UNITY_MATRIX_IT_MV, normal.xyzz).xyz);
				//o.worldNormal = viewN;
				return o;
			}
			
			
			float4 _RimColor;
			float _RimPower;

			sampler2D _CameraDepthNormalsTexture;
			fixed4 _Color;

			float triWave(float t, float offset, float yOffset)
			{
				return saturate(abs(frac(offset + t) * 2 - 1) + yOffset);
			}

			fixed4 texColor(v2f i, float rim)
			{
				fixed4 mainTex = tex2D(_MainTex, i.uv);
				mainTex.r *= triWave(_Time.x * 5, abs(i.objectPos.y) * 2, -0.7) * 6;
				// I ended up saturaing the rim calculation because negative values caused weird artifacts
				mainTex.g *= saturate(rim) * (sin(_Time.z + mainTex.b * 5) + 1);
				return mainTex.r * _Color + mainTex.g * _Color;
			}


			fixed4 frag (v2f i) : SV_Target
			{
				float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenuv).zw);
				float diff = screenDepth - i.depth;
				float intersect = 0;
				
				if (diff > 0)
					intersect = 1 - smoothstep(0, _ProjectionParams.w * 0.5, diff);

				float rim = 1 - abs(dot(i.normal, normalize(i.viewDir))) * 2;
				float northPole = (i.objectPos.y - 0.45) * 20;
				float glow = max(max(intersect, rim), northPole);

				fixed4 glowColor = fixed4(lerp(_Color.rgb, fixed3(1, 1, 1), pow(glow, 4)), 1);
				
				fixed4 hexes = texColor(i, rim);

				fixed4 col = _Color * _Color.a + glowColor * glow + hexes;
				return col;
			}
			ENDCG
		}
	}
}
