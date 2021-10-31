// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PetKnight/MarqueeShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
        [PerRendererData] _Color ("Color", Color) = (1,1,1,1)
        [PerRendererData] _Offset ("Offset", float) = 0
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass {
            CGPROGRAM

            #pragma vertex main_vertex
            #pragma fragment SpriteFrag

            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #include "UnitySprites.cginc"

            float _Offset;

            v2f main_vertex(appdata_t input)
            {
                v2f output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.vertex = UnityFlipSprite(input.vertex, _Flip);
                output.vertex = UnityObjectToClipPos(output.vertex);
                output.vertex = UnityPixelSnap(output.vertex);
                output.texcoord =input.texcoord;
                output.texcoord += float2(_Offset, 0);
                output.color = input.color * _Color * _RendererColor;

                return output;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
