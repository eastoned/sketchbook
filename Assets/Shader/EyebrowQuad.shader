Shader "Unlit/EyebrowQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Strokes ("Strokes", Range(0, 300)) = 1
        _Offset ("Offset", Range(0, 3.14)) = 0
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Strokes, _Offset;

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
                float2 uv = i.uv;
                float result = sin(uv.x * _Strokes * 3.14 + _Offset)/6 - uv.y + .75;
                float result2 = -sin(uv.x * _Strokes * 3.14 + _Offset)/6 - uv.y + 0.75;
                //float result2 = sin(
                
                return step(0, max(result2.xxxx, result.xxxx)*1-step(0, min(result2.xxxx, result.xxxx)));
            }
            ENDCG
        }
    }
}
