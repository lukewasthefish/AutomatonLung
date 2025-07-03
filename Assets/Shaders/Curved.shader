// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Curved" { Properties{ _MainTex("Color (RGB) Alpha (A)", 2D) = "white"

	_Sin1_Frequency("Frequency Of Wave 1", Float) = 0.12
	_Sin1_Offset("Offset Of Wave 1", Float) = 2.61
	_Sin1_Amplitude("Amplitude of Wave 1", Float) = 0.35

	_Sin2_Frequency("Frequency Of Wave 2", Float) = 0.46
	_Sin2_Offset("Offset Of Wave 2", Float) = 3.26
	_Sin2_Amplitude("Amplitude of Wave 2", Float) = 0.55
}
SubShader{
	Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		sampler2D _MainTex;

		uniform float _Sin1_Frequency;
		uniform float _Sin1_Offset;
		uniform float _Sin1_Amplitude;
		uniform float _Sin2_Frequency;
		uniform float _Sin2_Offset;
		uniform float _Sin2_Amplitude;

		struct v2f {
			float4 pos : SV_POSITION;
			float4 uv : TEXCOORD0;
			float4 posWorld : TEXCOORD1;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
			float zOff = vPos.z;
			float xOff = vPos.x;
			vPos += float4(0.0f, (zOff * sin(zOff * _Sin1_Frequency - _Sin1_Offset) * _Sin1_Amplitude) + (sin(zOff * _Sin2_Frequency - _Sin2_Offset) * _Sin2_Amplitude) + pow(abs(xOff), 2) * zOff * 0.0005, 0.0f, 0.0f);
			o.pos = mul(UNITY_MATRIX_P, vPos);
			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.uv = v.texcoord;
			return o;
		}

		half4 frag(v2f i) : COLOR
		{
			half4 col = tex2D(_MainTex, i.uv.xy);

			return col;
		}
		ENDCG
	}
}}