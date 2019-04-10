// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BasicScrollingShader"
{
	Properties
	{
		_MainTex("MainTexture", 2D) = "white" {}
		_XShift("Shift in the X direction", Float) = 0.1
		_YShift("Shift in the Y direction", Float) = 0.1
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent"  "PreviewType" = "Plane"}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Lighting Off

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				half _XShift;
				half _YShift;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					//get world space position
					o.uv = mul(unity_ObjectToWorld, v.vertex).xy;

					//do shifting
					o.uv.x += _XShift * _Time.gg;
					o.uv.y += _YShift * _Time.gg;

					o.uv = TRANSFORM_TEX(o.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					return col;
				}
				ENDCG
			}
		}
}