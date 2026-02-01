Shader "Sprites/TransparencyWhenPlayer"
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 ViewPos : TEXCOORD1;
            };

            sampler2D _MainTex, _AlphaTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.ViewPos = UnityObjectToViewPos(v.vertex).xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float transparency;
                // Player pos is always at (0.5,0.5) in screen space
                // Closer to player means more transparent


                float distToPlayer = length(i.ViewPos);
                transparency = saturate(lerp(0, 1, distToPlayer * 0.1));
                transparency *= tex2D(_MainTex, i.uv).a;
                transparency = saturate(transparency);
                //return float4(distToPlayer, 0, 0, 1);
                //return float4(transparency, transparency, transparency, transparency);
                //transparency += tex2D(_MainTex, i.uv).a;
                //transparency = saturate(transparency);
                //transparency = tex2D(_MainTex, i.uv).a;
                //return lerp(0, 1, distToPlayer);

                float3 col = tex2D(_MainTex, i.uv).rgb;
                //transparency = tex2D(_MainTex, i.uv).a;
                //col = transparency;
                //return float4(col, 1);
                //return transparency == 0;
                return float4(col * transparency, transparency);
            }

        ENDCG
        }
    }
}