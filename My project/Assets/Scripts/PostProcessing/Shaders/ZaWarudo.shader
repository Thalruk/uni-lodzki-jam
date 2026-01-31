Shader "PostProcessing/ZaWarudo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _SourceTex;
            float _TimeElapsed;

  
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 centerUV : TEXCOORD1;
            };

            float3 HSVtoRGB(float3 HSV)
            {
                float3 RGB = 0;
                float C = HSV.z * HSV.y;
                float H = HSV.x * 6;
                float X = C * (1 - abs(fmod(H, 2) - 1));
                if (HSV.y != 0)
                {
                    float I = floor(H);
                    if (I == 0) { RGB = float3(C, X, 0); }
                    else if (I == 1) { RGB = float3(X, C, 0); }
                    else if (I == 2) { RGB = float3(0, C, X); }
                    else if (I == 3) { RGB = float3(0, X, C); }
                    else if (I == 4) { RGB = float3(X, 0, C); }
                    else { RGB = float3(C, 0, X); }
                }
                float M = HSV.z - C;
                return RGB + M;
            }

            float3 RGBtoHSV(float3 RGB)
            {
                float3 HSV = 0;
                float M = min(RGB.r, min(RGB.g, RGB.b));
                HSV.z = max(RGB.r, max(RGB.g, RGB.b));
                float C = HSV.z - M;
                if (C != 0)
                {
                    HSV.y = C / HSV.z;
                    float3 D = (((HSV.z - RGB) / 6) + (C / 2)) / C;
                    if (RGB.r == HSV.z)
                        HSV.x = D.b - D.g;
                    else if (RGB.g == HSV.z)
                        HSV.x = (1.0/3.0) + D.r - D.b;
                    else if (RGB.b == HSV.z)
                        HSV.x = (2.0/3.0) + D.g - D.r;
                    if ( HSV.x < 0.0 ) { HSV.x += 1.0; }
                    if ( HSV.x > 1.0 ) { HSV.x -= 1.0; }
                }
                return HSV;
            }

            float3 InverseColorsAndChangeHue(float3 color)
            {
                //float3 inverse = float3(1.0 - color.r, 1.0 - color.g, 1.0 - color.b);
                float3 inverse = color;
                float3 hsv = RGBtoHSV(inverse);
                hsv.x += _TimeElapsed * 0.1;
                hsv.y -= _TimeElapsed - 1;
                
                float desat = saturate(1.0 - (_TimeElapsed * 0.1) + 2);
                hsv.y *= desat;
                
                float3 rgb = HSVtoRGB(hsv);
                return rgb;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 uv = v.uv; // standardowe UV
                o.uv = uv;

                uv = uv - 0.5;   // teraz (0,0) jest w œrodku

                float aspect = _ScreenParams.x / _ScreenParams.y;
                uv.x *= aspect;

                o.centerUV = uv;
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                // oryginal texture
                float3 oryginalCol = tex2D(_SourceTex, i.uv).xyz;
                float dist = length(i.centerUV);

                float mask = dist < _TimeElapsed;
                float3 col = lerp(oryginalCol, InverseColorsAndChangeHue(oryginalCol), mask);
                //return mask;
                return float4(col, 1.0);
                
            }
            ENDCG
        }
    }
}
