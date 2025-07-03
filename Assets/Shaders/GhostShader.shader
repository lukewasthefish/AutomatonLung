// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "N3DS/Outlined/Ghost" {
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_MainColor ("Main Color", Color) = (0,0,0,0.6)
		_Outline ("Outline width", Range (0.0, 0.2)) = .005
		_Frequency ("Frequency", Range (1.0, 2.0)) = 1.0
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
uniform float4 _MainColor;
uniform float _Frequency;
 
v2f vert(appdata v) {
	// just make a copy of incoming vertex data but scaled according to normal direction
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);

	float4 node_7725 = _Time;

	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * (cos(node_7725.g*_Frequency) * 0.5 * _Outline + _Outline);
	o.color = _OutlineColor;

	return o;
}
ENDCG
 
	SubShader {
		Tags { "Queue" = "Transparent" }
 
		Pass {
			Name "BASE"
			Cull Back
			Blend Zero One
 
			// uncomment this to hide inner details:
			Offset -8, -8
 
			SetTexture [_MainColor] {
				ConstantColor (0,0,0,0)
				Combine constant
			}
		}
 
		// note that a vertex shader is specified here but its using the one above
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
 
			// you can choose what kind of blending mode you want for the outline
			//Blend SrcAlpha OneMinusSrcAlpha // Normal
			//Blend One One // Additive
			Blend One OneMinusDstColor // Soft Additive
			//Blend DstColor Zero // Multiplicative
			//Blend DstColor SrcColor // 2x Multiplicative
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			 
			half4 frag(v2f i) :COLOR {
				return i.color;
			}
			ENDCG
		}
 
 
	}
 
	Fallback "Diffuse"
}