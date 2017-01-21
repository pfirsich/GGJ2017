Shader "Unlit/sonar"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SonarColor ("sonarColor", Color) = (1.0, 1.0, 1.0, 1.0)
		_SonarFrontWidth ("sonarFrontWidth", Float) = 1.0
		_SonarTailWidth ("sonarTailWidth", Float) = 4.0
		_SonarConeFactorBias ("sonarConeFactorBias", Float) = 0.9
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SimplexNoise3D.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 worldSpacePos : TEXCOORD6;
				float4 viewSpacePos : TEXCOORD7;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			#define MAX_SONAR_COUNT 8
			float _SonarRadius[MAX_SONAR_COUNT];
			float4 _SonarDirection[MAX_SONAR_COUNT];
			float4 _SonarColor;
			float _SonarFrontWidth;
			float _SonarTailWidth;
			float _SonarConeFactorBias;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldSpacePos = mul(UNITY_MATRIX_M, v.vertex);
				o.viewSpacePos = mul(UNITY_MATRIX_MV, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float sonarEnvelope(float x, float frontWidth, float tailWidth) {
				float env = 1.0 - abs(x);
				float width;
				if(x > 0.0) {
					width = frontWidth;
				} else {
					width = tailWidth;
				}
				return saturate((env + width - 1.0) / width);
			}

			float sonar(float4 viewSpacePos, float radius, float frontWidth, float tailWidth, float4 direction, float coneFactorBias) {
				float brightnessTerm = sonarEnvelope(length(viewSpacePos) - radius, frontWidth, tailWidth);

				float coneFactor = saturate(dot(normalize(viewSpacePos), direction));
				coneFactor = saturate((coneFactor - coneFactorBias) / (1.0 - coneFactorBias));

				return brightnessTerm;// * coneFactor;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float brightness = 0.0;
				for(int s = 0; s < MAX_SONAR_COUNT; ++s) {
					brightness += sonar(i.viewSpacePos, _SonarRadius[s], _SonarFrontWidth, _SonarTailWidth, _SonarDirection[s], _SonarConeFactorBias);
				}
				//brightness = sonar(i.viewSpacePos, _SonarRadius, _SonarFrontWidth, _SonarTailWidth, _SonarDirection, _SonarConeFactorBias);

				float t = _Time.w * 0.015;
				float thickness = 0.01;
				float _noise = snoise(i.worldSpacePos * 0.05 + float3(1, 1, 0) * t);
				float waveEffect = 0.0;
				const int steps = 30;
				for(int w = -steps + 1; w < steps; ++w) {
					waveEffect += sonarEnvelope(_noise + 1.0/steps*w, thickness, thickness);
				}
				brightness *= saturate(waveEffect);

				fixed4 col = float4(float3(brightness, brightness, brightness), 1.0) * _SonarColor; //tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
