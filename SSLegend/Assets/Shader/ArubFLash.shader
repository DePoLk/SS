Shader "Custom/ArubFLash"
{
	Properties{
		 _NotVisibleColor("NotVisibleColor (RGB)", Color) = (0.3,0.3,0.3,1)
		 _Color("Color", Color) = (0,0,0,1)
		 _MainTex("Albedo (RGB)", 2D) = "white" {}
		 _MetallicTex("Metallic Texture", 2D) = "white" {}
		 _Metallic("Metallic", Range(0,1)) = 0.0
		 _RoughnessTex("Roughness Texture", 2D) = "white" {}
		 _Glossiness("Smoothness", Range(0,1)) = 0.5

		
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Opaque" = "Geomatry"}
			LOD 100
			
			CGPROGRAM

			 #pragma surface surf Standard fullforwardshadows


					//#pragma surface surf Lambert
					sampler2D _MainTex;
					sampler2D _MetallicTex;
					sampler2D _RoughnessTex;
					half _Glossiness;
					half _Metallic;
					half _Roughness;
					fixed4 _Color;


					struct Input {
						float2 uv_MainTex;
						float2 uv_MetallicTex;
						float2 uv_RoughnessTex;
					};


					void surf(Input IN, inout SurfaceOutputStandard o) {
						// Albedo comes from a texture tinted by color

						fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
						o.Albedo = c.rgb;
						// Metallic and smoothness come from slider variables
						o.Metallic = (tex2D(_MetallicTex,IN.uv_MetallicTex)) * _Metallic;
						o.Smoothness = (tex2D(_RoughnessTex,IN.uv_RoughnessTex)) * _Glossiness;

						o.Alpha = c.a;
					}
					ENDCG
		}
			FallBack "Diffuse"
}
