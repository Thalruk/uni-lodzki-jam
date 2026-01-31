Shader "Mask/Write"
{
    SubShader
    {
        Tags { "Queue"="Overlay" }
        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off
            Blend One One // ADD (akumulacja)

            ColorMask R

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv * 2 - 1;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return length(i.uv) <= 1;
            }
            ENDCG
        }
    }
}
