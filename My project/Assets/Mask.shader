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
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag () : SV_Target
            {
                return 1;
            }
            ENDCG
        }
    }
}
