Shader "Luke/WaterWave" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }

			// Pass to render object as a shadow caster
			Pass
			{
				//ZWrite On ZTest LEqual Cull Off
				Cull Off	
				
				CGPROGRAM
				#pragma vertex vertexFunc
				#pragma fragment fragmentFunc

				float4 _Color;
				float _Strength;
				float _Speed;

				struct vertexInput {
					float4 vertex : POSITION;
				};

				struct vertexOutput {
					float4 pos : SV_POSITION;
				};

				vertexOutput vertexFunc(vertexInput IN) {
					vertexOutput o;

					float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);
					float distanceFromObject = distance(worldPos, _WorldSpaceCameraPos);

					worldPos.y = (worldPos.y - distanceFromObject);

					o.pos = mul(UNITY_MATRIX_VP, worldPos);

					return o;
				}

				float4 fragmentFunc(vertexOutput IN) : COLOR{
					return _Color;
				}

				ENDCG
			}
	}
}
