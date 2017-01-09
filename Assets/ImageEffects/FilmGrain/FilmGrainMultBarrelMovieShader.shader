Shader "Hidden/FilmGrainMultBarrelMovieShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile SAT_GRAIN_FACE SAT_GRAIN_SCROLLING SAT_GRAIN_MULT SAT_GRAIN_MOVIE SAT_GRAIN_JITTER
			
			#if defined (SAT_GRAIN_FACE)
			#define HAS_FACE 1
			#endif

			#if defined (SAT_GRAIN_SCROLLING)
			#define HAS_SCROLLING 1
			#endif
			
			#if defined (SAT_GRAIN_MULT)
			#define HAS_MULT 1
			#endif

			#if defined (SAT_GRAIN_MOVIE)
			#define HAS_MOVIE 1
			#endif
			
			#if defined (SAT_GRAIN_JITTER)
			#define HAS_JITTER 1
			#endif
			

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			float2 Pack(float2 param)
			{
				return param * 0.5 + 0.5;
			}

			float2 Unpack(float2 param)
			{
				return param + param - 1.0;
			}

			float rand(float2 co)
			{
				return frac(sin(1000.0*dot(co.xy, float2(21.5739, 43.421))) * 617284.3);
			}

			float rand(float co)
			{
				return frac(sin(1000.0 * co) * 617284.3);
			}

			float2 reverseMirrorFlip(float2 input)
			{
				input += 2.0f;
				if ((uint)input.x % 2 == 0)
				{
					input.x = frac(input.x);
				}
				else
				{
					input.x = 1.0 - frac(input.x);
				}
				if ((uint)input.y % 2 == 0)
				{
					input.y = frac(input.y);
				}
				else
				{
					input.y = 1.0 - frac(input.y);
				}
				return input;
			}

			half uDistortion = 1.2f;
			half uZoom = 1.0f;

			float2 BarrelDistortion(float2 texcoord)
			{
				texcoord = Unpack(texcoord);
				texcoord *= uZoom;
				float theta = atan2(texcoord.y, texcoord.x);
				float radius = length(texcoord);
				radius = pow(radius, uDistortion);
				texcoord.x = radius * cos(theta);
				texcoord.y = radius * sin(theta);
				texcoord = 0.5 * (texcoord + 1.0);
				
				return texcoord;
			}



			sampler2D _MainTex;
			float uAmount;
			float RandomNumber;
			float2 uOffsetAmount;

		#if defined (SAT_GRAIN_FACE)
			sampler2D uSpookyFaceTexture;
			float uSpookyAmount;
		#endif

		#if defined (SAT_GRAIN_SCROLLING)
			sampler2D uScrollingTexture;
			sampler2D uScrollingGlitchTexture;
			float2 uScrollAmount;
		#endif

		//#if defined (SAT_GRAIN_MULT)
			sampler2D uMultTexture;
		//#endif

		#if defined (SAT_GRAIN_MOVIE)
			sampler2D uMovie;
			float uMovieAmount;
		#endif
			
		#if defined (SAT_GRAIN_SCROLLING)
			float uScrollingGlitchAmount;
		#endif
			
		//#if defined (SAT_GRAIN_JITTER)
			float4 jitterParam;
			
#define jitterRandomNumber jitterParam.xy
#define jitterAmount jitterParam.zw
		//#endif
			
			

			fixed4 frag (v2f i) : SV_Target
			{				

				//mirrorFlipUV = Distort(mirrorFlipUV); 
				float2 barrel = 0.5f;

			//#if defined (SAT_GRAIN_JITTER)
				float2 jitterOffset = float2(rand(jitterRandomNumber.xx + i.uv), rand(jitterRandomNumber.yy + i.uv));
				jitterOffset = jitterOffset + jitterOffset - 1.0f;
				jitterOffset = jitterOffset * jitterAmount / _ScreenParams.xy;
				barrel = BarrelDistortion(i.uv + jitterOffset + uOffsetAmount);	
			//#else
			//	barrel = BarrelDistortion(i.uv + uOffsetAmount);				
			//#endif

				float2 mirrorFlipUV = reverseMirrorFlip(barrel);
				fixed4 col = tex2D(_MainTex, mirrorFlipUV);
			
			#if HAS_FACE
				col.rgb = lerp(col.rgb, tex2D(uSpookyFaceTexture, mirrorFlipUV).rgb, uSpookyAmount);
			#endif
				
				float2 RandomNumber2 = RandomNumber.rr;
				float aspectRatio = _ScreenParams.x / _ScreenParams.y * 100.0f;

				float2 uvRound = floor(i.uv * 100.0f) / 100.0f; 
				uvRound.x = floor(i.uv.x * aspectRatio) / aspectRatio;
				uvRound.y = floor(i.uv.y * 100.0f) / 100.0f;
				float3 filmGrain;
				filmGrain.x = rand(RandomNumber);
				col.rgb = lerp(col.rgb, 
					float3(rand(RandomNumber + uvRound), rand(RandomNumber + uvRound), rand(RandomNumber + uvRound)),
					uAmount);

			#if HAS_SCROLLING
				float4 scrollTex = tex2D(uScrollingTexture, i.uv.xy + uScrollAmount); 
				col.rgb = lerp(col.rgb, scrollTex.rgb, scrollTex.a * uAmount * 1.75f);
				float4 scrollGlitchTex = tex2D(uScrollingGlitchTexture, frac(i.uv.xy + uScrollAmount + float2(0.0f, rand(RandomNumber))));
				col.rgb = lerp(col.rgb, scrollGlitchTex.rgb, scrollGlitchTex.a * uScrollingGlitchAmount);
			#endif
				
			#if HAS_MOVIE
				fixed3 MovieCol = tex2D(uMovie, i.uv).rgb;
				col.rgb += MovieCol * uMovieAmount;
			#endif
			
			//#if HAS_MULT
				col.rgb *= tex2D(uMultTexture, i.uv);
			//#endif
				
				//Clamp the barrel
				//if (barrel.x > 1.0f || barrel.x < 0.0f || barrel.y > 1.0f || barrel.y < 0.0f)
				//{
				//	col.rgb = float3(0.0, 0.0, 0.0);
				//}

				return col;

			}
			ENDCG
		}
	}
}
