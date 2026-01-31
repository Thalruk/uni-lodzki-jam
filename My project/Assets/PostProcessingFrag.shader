Shader "Unlit/PostProcessingFrag"
{
    Properties
    {
        _Scale ("Scale", float) = 1
        _Offset ("Offset", Vector) = (0,0,0,0)
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

            sampler2D _SourceTex;
            float _Scale;
            float4 _Offset;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 uv = v.uv;
                uv -= 0.5;
                uv *= 2;
                uv *= _Scale;
                uv *= 0.5;
                uv += 0.5;
                float2 offset;
                offset.x = min(abs(_Offset.x), (1 - _Scale) * 0.5) * sign(_Offset.x);
                offset.y = min(abs(_Offset.y), (1 - _Scale) * 0.5) * sign(_Offset.y);
                uv += offset;
                o.uv = uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_SourceTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
