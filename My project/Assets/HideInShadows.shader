Shader "Sprites/HideInShadows"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"

            sampler2D _ScreenMaskTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.vertex.xyz / o.vertex.w;
                float2 screenUV = o.screenPos.xy * 0.5 + 0.5;
                screenUV.y = 1.0 - screenUV.y;
                o.screenPos = screenUV;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float transparency = tex2D(_MainTex, i.uv).a;
                transparency *= tex2D(_ScreenMaskTexture, i.screenPos).r;
                //return transparency == 0;
                transparency = saturate(transparency);
                //return float4(transparency, transparency, transparency, 1);
                float3 col = tex2D(_MainTex, i.uv).rgb;
                return float4(col * transparency, transparency);
                return tex2D(_ScreenMaskTexture, i.screenPos).r;
                return float4(i.screenPos, 0, 1);
                return transparency;
                return float4(1, 0, 0, 1);
            }

        ENDCG
        }
    }
}