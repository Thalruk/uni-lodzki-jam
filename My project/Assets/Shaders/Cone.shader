Shader "Unlit/Cone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }



    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        LOD 100

        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off

            ColorMask R

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _ViewRange;

            float easeInQuint(float x) 
            {
                return 1 - sqrt(1 - pow(x, 2));
            }

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 objPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.objPos = v.vertex.xy;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float len = length(i.objPos);
                float maxLen = _ViewRange;

                float3 col = float3(0, 0, 0);
                float transparency = 1 - easeInQuint(len / maxLen);//len < maxLen;
                return 1 * transparency;
            }
            ENDCG
        }

        Pass
        {
            Blend Zero One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 objPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Fov;
            float _ViewRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.objPos = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float len = length(i.objPos);
                float maxLen = _ViewRange;

                float3 col = float3(0.1, 0.1, 0.1);
                float transparency = len < maxLen;
                return float4(col, transparency);
            }
            ENDCG
        }
    }
}
