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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			// FUCK ALL OF UNITY. DIE IN HELL
			float _SonarRadius;
			float4 _SonarDirection;
			float4 _SonarPosition;
			float _SonarRadius1;
			float4 _SonarDirection1;
			float4 _SonarPosition1;
			float _SonarRadius2;
			float4 _SonarDirection2;
			float4 _SonarPosition2;
			float _SonarRadius3;
			float4 _SonarDirection3;
			float4 _SonarPosition3;
			float _SonarRadius4;
			float4 _SonarDirection4;
			float4 _SonarPosition4;
			float _SonarRadius5;
			float4 _SonarDirection5;
			float4 _SonarPosition5;
			float _SonarRadius6;
			float4 _SonarDirection6;
			float4 _SonarPosition6;
			float _SonarRadius7;
			float4 _SonarDirection7;
			float4 _SonarPosition7;


			float4 _SonarColor;
			float _SonarFrontWidth;
			float _SonarTailWidth;
			float _SonarConeFactorBias;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldSpacePos = mul(UNITY_MATRIX_M, v.vertex);
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

			float sonar(float4 worldSpacePos, float4 center, float radius, float frontWidth, float tailWidth, float4 direction, float coneFactorBias) {
				float brightnessTerm = sonarEnvelope(length(worldSpacePos - center) - radius, frontWidth, tailWidth);

				float coneFactor = saturate(dot(normalize(worldSpacePos), direction));
				coneFactor = saturate((coneFactor - coneFactorBias) / (1.0 - coneFactorBias));

				return brightnessTerm;// * coneFactor;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float brightness = 0.0;
				brightness += sonar(i.worldSpacePos, _SonarPosition, _SonarRadius, _SonarFrontWidth, _SonarTailWidth, _SonarDirection, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition1, _SonarRadius1, _SonarFrontWidth, _SonarTailWidth, _SonarDirection1, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition2, _SonarRadius2, _SonarFrontWidth, _SonarTailWidth, _SonarDirection2, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition3, _SonarRadius3, _SonarFrontWidth, _SonarTailWidth, _SonarDirection3, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition4, _SonarRadius4, _SonarFrontWidth, _SonarTailWidth, _SonarDirection4, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition5, _SonarRadius5, _SonarFrontWidth, _SonarTailWidth, _SonarDirection5, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition6, _SonarRadius6, _SonarFrontWidth, _SonarTailWidth, _SonarDirection6, _SonarConeFactorBias);
				brightness += sonar(i.worldSpacePos, _SonarPosition7, _SonarRadius7, _SonarFrontWidth, _SonarTailWidth, _SonarDirection7, _SonarConeFactorBias);
				//brightness += sonar(i.worldSpacePos, _SonarRadius2, _SonarFrontWidth, _SonarTailWidth, _SonarDirection2, _SonarConeFactorBias);
				//brightness += sonar(i.worldSpacePos, _SonarRadius3, _SonarFrontWidth, _SonarTailWidth, _SonarDirection3, _SonarConeFactorBias);

				//brightness = sonar(i.worldSpacePos, _SonarRadius, _SonarFrontWidth, _SonarTailWidth, _SonarDirection, _SonarConeFactorBias);

				float t = _Time.w * 0.015;
				float thickness = 0.002;
				float _noise = snoise(i.worldSpacePos * 0.01 + float3(1, 1, 0) * t);
				float waveEffect = 0.0;
				const int steps = 30;
				for(int w = -steps + 1; w < steps; ++w) {
					waveEffect += sonarEnvelope(_noise + 1.0/steps*w, thickness, thickness);
				}
				brightness *= saturate(waveEffect) * 3.0;

				fixed4 col = float4(float3(brightness, brightness, brightness), 1.0) * _SonarColor; //tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
