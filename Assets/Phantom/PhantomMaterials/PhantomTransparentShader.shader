Shader "Phantom/PhantomTransparentShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalAmount("Distortion", range(0,128)) = 16
		_RimColor("Rim Color", Color) = (0.4,0.4,0.4,0.0)
		_RimPower("Rim Power", Range(0.0,8.0)) = 3.0
		_RimAlpha("Rim Amount", Range(0.0,1.0)) = 1.0
		_RefractionSpeed("Distortion Speed", Vector) = (1.0, 2.0, 0.82, -1.55) 
		_RefractionRim("Distortion Rim", range(0,1)) = 1.0 
	}

	Category
	{

		//Be transparent so everything else gets drawn before
		//Tags 
		//{ 
		//	"RenderType" = "Transparent" 
		//	"IgnoreProjector" = "True" 
		//	"Queue" = "Transparent"
		//}

		Tags
		{ 
			"Queue" = "Transparent" 
			"RenderType" = "Opaque" 
		}

		//LOD 100
		//ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha
		//Blend SrcAlpha One

		SubShader
		{
			// This gets us the scene and puts it into _GrabTexture
			GrabPass
			{
				Name "BASE"
				Tags{ "LightMode" = "Always" }
			}

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
					float2 uvMain : TEXCOORD0;
					float2 uvNorm : TEXCOORD1;
					float4 uvGrab : TEXCOORD2;
					half3 normal : TEXCOORD3;
					half4 randomRim : TEXCOORD4;
					float3 viewDir : TEXCOORD5;
					float4 vertex : SV_POSITION;
					//float3 viewDirection;
				};

				//GrabTexture contains the scene fragments to be used in the refraction
				sampler2D _GrabTexture;
				float4 _GrabTexture_TexelSize;


				sampler2D _MainTex;
				sampler2D _NormalMap;


				float4 _MainTex_ST;
				float4 _NormalMap_ST;
				float _NormalAmount;


				v2f vert(appdata v, float3 normal : NORMAL)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					o.viewDir = normalize(ObjSpaceViewDir(v.vertex));

					//If UVs are fucked, unfuck them
				#if UNITY_UV_STARTS_AT_TOP
					float flip = -1.0;
				#else
					float flip = 1.0;
				#endif
					o.uvGrab.xy = (float2(o.vertex.x, o.vertex.y * flip) + o.vertex.w) * 0.5;
					o.uvGrab.zw = o.vertex.zw;
					
					o.uvMain = TRANSFORM_TEX(v.uv, _MainTex);
					o.uvNorm = TRANSFORM_TEX(v.uv, _NormalMap);
					
					o.normal = normal;

					float vertexAdjust = (v.vertex.x + v.vertex.y + v.vertex.z);

					o.randomRim.r = sin((v.vertex.x + v.vertex.y + v.vertex.z) * 50.0f + _Time.g * 10.0f * 1.0f) * 0.3 + 1.0;
					o.randomRim.g = cos((v.vertex.x + v.vertex.y + v.vertex.z) * 90.0f + _Time.g * 10.0f * 1.2f) * 0.3 + 1.0;
					o.randomRim.b = cos((v.vertex.x + v.vertex.y + v.vertex.z) * 75.0f + _Time.g * 10.0f * 1.1f) * 0.3 + 1.0;
					o.randomRim.a = sin((v.vertex.x + v.vertex.y + v.vertex.z) * 60.0f + _Time.g * 10.0f * 0.9f) * 0.3 + 1.0;
					
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}


				float4 _RimColor;
				float _RimPower;
				float _RimAlpha;
				float4 _RefractionSpeed;
				float _RefractionRim;

				half4 frag(v2f i) : SV_Target
				{
					float3 normalDirection = normalize(i.normal);
					float3 viewDirection = normalize(i.viewDir);

					half rim = saturate(dot(viewDirection, normalDirection));
					//half rimAmount = pow(rim, _RimPower * i.randomRim.r);
					half rimAmount = pow(1.0 - rim, _RimPower * i.randomRim.a);
					rimAmount *= _RimAlpha;

					half2 norm = (
					UnpackNormal(tex2D(_NormalMap, i.uvNorm + float2(_Time.r * _RefractionSpeed.x, _Time.r * _RefractionSpeed.y))).rg +
					UnpackNormal(tex2D(_NormalMap, i.uvNorm + float2(_Time.r * _RefractionSpeed.z, _Time.r * _RefractionSpeed.w))).rg) * 0.5f;
					float2 offset = norm * _NormalAmount * _GrabTexture_TexelSize.xy;

					i.uvGrab.xy += offset * i.uvGrab.z * lerp(rim * rim * rim, 1.0f, _RefractionRim);

					half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvGrab)) 
						* tex2D(_MainTex, i.uvMain);
										
					//col.a = rimAmount;
					col.a = 1.0f;
					col.rgb += _RimColor.rgb * i.randomRim.rgb * rimAmount;

					return col;
				}
				ENDCG
			}
		}
	}
}
