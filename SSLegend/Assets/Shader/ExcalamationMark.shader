// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Excalamation"
{
	Properties
	{
		_MainTex("Main Texture (RGB)", 2D) = "white" {}                     //  主貼圖  //
		_MainColorTimes("MainColorTimes", Range(0, 30)) = 1                 //  主圖顏色增強倍數  //
		_EmissionTex("_EmissionTex", 2D) = "white" {}                       //  光暈Alpha圖,取Alpha值填補Emission顏色    //
		_EmissionAlphaTimes("EmissionAlphaTimes", Range(0, 50)) = 1         //  光暈Alpha增強倍數 //
		_EmissionAlphaExponent("EmissionAlphaExponent", Range(0, 10)) = 1   //  光暈Alpha指數，用於消除黑邊    //

		_Emission1("Emmisive Color1", Color) = (0,0,0,0)                    //  劍體本身的發光顏色   //
		_EmissionColorTimes1("EmissionColorTimes1", float) = 1              //  劍體本身的發光顏色倍數 //

		_Emission2("Emmisive Color2", Color) = (0,0,0,0)                    //  劍體光暈的發光顏色   //
		_EmissionColorTimes2("EmissionColorTimes2", float) = 1              //  劍體光暈的發光顏色倍數 //

		_AllAlpha("AllAlpha", Range(0, 1)) = 1                              //  整體Alpha值    //
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent+100"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}

			LOD 100
			Cull Off
			ZWrite Off
			AlphaTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			Fog{ Mode Off }

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				uniform fixed _MainColorTimes;
				sampler2D _EmissionTex;
				uniform float _EmissionAlphaTimes;
				uniform float _EmissionAlphaExponent;
				uniform fixed4 _Emission1;
				uniform fixed _EmissionColorTimes1;
				uniform fixed4 _Emission2;
				uniform fixed _EmissionColorTimes2;
				uniform fixed _AllAlpha;

				struct appdata_t
				{
					float4 vertex   : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord  : TEXCOORD;
				};

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color;

					return OUT;
				}

				fixed4 frag(v2f IN) : COLOR
				{
					fixed4 mainColor = tex2D(_MainTex, IN.texcoord);
					fixed4 emisionColor = tex2D(_EmissionTex, IN.texcoord);

					mainColor.rgb = mainColor.rgb * _MainColorTimes
						+ _Emission1 * mainColor.a * _EmissionColorTimes1
						+ _Emission2 * (1 - mainColor.a) * emisionColor.a * _EmissionColorTimes2;

					mainColor.a = max(mainColor.a, pow(emisionColor.a, _EmissionAlphaExponent)
					 * _EmissionAlphaTimes) * _AllAlpha;

					return  mainColor;
				}
				ENDCG
			}
		}
}