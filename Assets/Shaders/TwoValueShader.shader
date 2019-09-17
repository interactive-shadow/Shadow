Shader "Unlit/TwoValueShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Threshould("Threshould", Float) = 0.22
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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
            };

			float3 rgb2hsv(float3 rgb)
			{
				float3 hsv;

				// RGBの三つの値で最大のもの
				float maxValue = max(rgb.r, max(rgb.g, rgb.b));
				// RGBの三つの値で最小のもの
				float minValue = min(rgb.r, min(rgb.g, rgb.b));
				// 最大値と最小値の差
				float delta = maxValue - minValue;

				// V（明度）
				// 一番強い色をV値にする
				hsv.z = maxValue;

				// S（彩度）
				// 最大値と最小値の差を正規化して求める
				if (maxValue != 0.0) {
					hsv.y = delta / maxValue;
				}
				else {
					hsv.y = 0.0;
				}

				// H（色相）
				// RGBのうち最大値と最小値の差から求める
				if (hsv.y > 0.0) {
					if (rgb.r == maxValue) {
						hsv.x = (rgb.g - rgb.b) / delta;
					}
					else if (rgb.g == maxValue) {
						hsv.x = 2 + (rgb.b - rgb.r) / delta;
					}
					else {
						hsv.x = 4 + (rgb.r - rgb.g) / delta;
					}
					hsv.x /= 6.0;
					if (hsv.x < 0)
					{
						hsv.x += 1.0;
					}
				}

				return hsv;
			}

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Threshould;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

				col.rgb = rgb2hsv(col.rgb);

				col = col.y < _Threshould ? 1 : 0;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
