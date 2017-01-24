Shader "Unlit/DoorFogShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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
			Tags
			{
				"WaterMode" = "Refractive" 

				"Queue" = "Transparent"
				"RenderType" = "Opaque"
			}

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
					float4 ref : TEXCOORD6;
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

					o.ref = ComputeScreenPos(mul(UNITY_MATRIX_MVP, v.vertex));
	
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
	
				
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}


				float4 _RefractionSpeed;
				float _RefractionRim;

				sampler2D _CameraDepthTexture;

				fixed4 frag(v2f i) : SV_Target
				{
					float3 normalDirection = normalize(i.normal);
					float3 viewDirection = normalize(i.viewDir);


					half2 norm = (
						UnpackNormal(tex2D(_NormalMap, i.uvNorm + float2(_Time.r * _RefractionSpeed.x, _Time.r * _RefractionSpeed.y))).rg +
						UnpackNormal(tex2D(_NormalMap, i.uvNorm + float2(_Time.r * _RefractionSpeed.z, _Time.r * _RefractionSpeed.w))).rg) * 0.5f;
					float2 offset = norm * _NormalAmount * _GrabTexture_TexelSize.xy * 100.0f;


					float4 GrabUV = i.uvGrab;

					float depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, i.ref).r) - i.ref.w;
					float shortDepth = min(depth * 0.5f, 10.0f);
					GrabUV.xy += shortDepth * (offset * i.uvGrab.z);
					float newDepth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, GrabUV).r) - i.ref.w;
					if (newDepth > 0.0f)
						depth = newDepth;

					float4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(GrabUV));
					col = lerp(col, col * tex2D(_MainTex, i.uvMain + float2(0.0f, _Time.g + i.uvMain.x * 0.5f + 0.1f * sin(i.uvMain.x * 3.1415f * 5.0f))), shortDepth);

					//col.a = rimAmount;
					col.a = 1.0f;
					//col.rgb += _RimColor.rgb * i.randomRim.rgb * rimAmount;

					col.rgb = min(col - pow(min(depth * 0.015f, 1.0f), 0.75f), lerp(1.0f, 0.5f, depth * 0.01f));

					//col.rgb = float3(i.uvGrab.x / _GrabTexture_TexelSize.x, i.uvGrab.y / _GrabTexture_TexelSize.y, 0.0f);
					//col.rgb = 0.0f;
					//col.rg = i.ref.rg;
					//col.rgb = depth * 0.1f;

					return col;
				}
			
				ENDCG
			}
		}
	}
}