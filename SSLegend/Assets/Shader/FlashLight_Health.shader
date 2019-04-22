Shader "Custom/FlashLight_Health"
{
	Properties{
		 _NotVisibleColor("NotVisibleColor (RGB)", Color) = (0.3,0.3,0.3,1)
		 _Color("Color", Color) = (170,250,200,1)
		 _MainTex("Albedo (RGB)", 2D) = "white" {}
		 _Glossiness("Smoothness", Range(0,1)) = 0.5
		 _Metallic("Metallic", Range(0,1)) = 0.0

		_OutlineTex("Outline Texture",2D) = "white" {}
		_OutlineColor_H("Outline Color_H", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1.0, 10.0)) = 1.1
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Opaque" = "AlphaTest"}
			LOD 200
			Pass {
				ZTest Greater
				ZWrite Off
				Color[_NotVisibleColor]
			}
			 Pass
			{
				Name "OUTLINE"

				ZWrite Off

				CGPROGRAM

				#pragma vertex vert //Define for the buliding function.

				#pragma fragment frag //Define for color function.



				#include "UnityCG.cginc"//Built in shader functions.


				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};
				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};


				float _OutlineWidth;
				float4 _OutlineColor_H;
				sampler2D _OutlineTex;



				v2f vert(appdata IN)
				{
					IN.vertex.xyz *= _OutlineWidth;
					v2f OUT;

					OUT.pos = UnityObjectToClipPos(IN.vertex);
					OUT.uv = IN.uv;

					return OUT;

				}


				fixed4 frag(v2f IN) : SV_Target
				{
					float4 texColor = tex2D(_OutlineTex, IN.uv);//Wraps the texture around the uv's.
					return texColor * _OutlineColor_H;//Tints the texture.
				}


				ENDCG
			}



			CGPROGRAM

			 #pragma surface surf Standard fullforwardshadows


					//#pragma surface surf Lambert
					sampler2D _MainTex;
					sampler2D _MetallicTex;
					half _Glossiness;
					half _Metallic;
					fixed4 _Color;


					struct Input {
						float2 uv_MainTex;
					};


					void surf(Input IN, inout SurfaceOutputStandard o) {
						// Albedo comes from a texture tinted by color
						fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color + _Color;
						o.Albedo = c.rgb;
						// Metallic and smoothness come from slider variables
						o.Metallic = _Metallic;
						o.Smoothness = _Glossiness;
						o.Alpha = c.a;
					}
					ENDCG
		}
			FallBack "Diffuse"
}
