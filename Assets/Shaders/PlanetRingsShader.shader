Shader "Custom/PlanetRingShader"
{
	Properties{
	   _MainTex("Main Texture", 2D) = "white" {}
	   _Color("Color", Color) = (0,0,0,0)
	   _Thickness("Thickness", Range(0.0,0.5)) = 0.05
	   _Radius("Radius", Range(0.0, 0.5)) = 0.4
	   _Dropoff("Dropoff", Range(0.01, 0.1)) = 0.05
	}
	SubShader{
		Pass {

		    Cull off // Render both sides
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
		    ZWrite Off  // Make Plane transparent
		    LOD 200 // Level of detail

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

		    sampler2D _MainTex;
			fixed4 _Color;
			float _Thickness;
			float _Radius;
			float _Dropoff;

			struct fragmentInput {
				float4 pos : SV_POSITION;
				float2 uv : TEXTCOORD0;
			};

			fragmentInput vert(appdata_base v)
			{
				fragmentInput o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy - fixed2(0.5,0.5);

				return o;
			}

			// r = radius
			// d = distance
			// t = thickness
			// p = % thickness used for dropoff

			float antialias(float r, float d, float t, float p) {
				float as = 0.0;
				if (d <= (r - 0.5*t)) {
					as = 1.0 - (pow(d - r + 0.5*t, 2) / pow(p*t, 2));
				}
				else if (d >= (r + 0.5*t)) {
					as = 1.0 - (pow(d - r - 0.5*t, 2) / pow(p*t, 2));
				}
				else if (d > r - 0.5 && d < r + 0.5) {
					as = 1.0;
				}
				return as;
			}

			fixed4 frag(fragmentInput i) : SV_Target {
				float distance = sqrt(pow(i.uv.x, 2) + pow(i.uv.y, 2));
			
				float start = _Radius - 0.6*_Thickness;
				float end = _Radius + 0.6*_Thickness;

				if (distance >= start && distance <= end) {

					float2 position = (0, (distance - start) * 4);
					half4 color = tex2D(_MainTex, position);

					return fixed4(color.r, color.g, color.b, color.a * antialias(_Radius, distance, _Thickness, _Dropoff));
				}
				else {
					return fixed4(0.0, 0.0, 0.0, 0.0);
				}
			}

			ENDCG
		}
	}

}
