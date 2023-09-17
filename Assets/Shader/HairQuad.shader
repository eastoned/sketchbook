Shader "Unlit/HairQuad"
{
    Properties
    {
        _BangRoundness ("Band Roundness", Range(0.25, 4)) = 1
        _StrandCount ("Strand Count", Range(0, 20)) = 2
        _StrandOffset ("Strand Offset", Range(0, 1)) = 0
        _HairBangScale ("Hair Bang Scale", Range(0.5, 2)) = 0
        _HairRoundness ("Hair Roundness", Range(1, 5)) = 2

        _Color1("Base", Color) = (1,1,1,1)
        _Color2("Accent", Color) = (0.5,0.5,0.5, 1)
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

            float _BangRoundness, _HairBangScale;
            int _StrandCount;
            int _StrandOffset;
            float _HairRoundness;
            float4 _Color1, _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z)/60, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                uv.x *= _StrandCount;
                uv.x += (0.5*_StrandOffset);
                uv = frac(uv);
                uv.y = pow(uv.y, _HairBangScale);
                
                float bang = pow(abs(i.uv.x*2-1), _HairRoundness) + pow(abs(uv.y*2-1), _HairRoundness);
                bang = (1-step(1, bang)) * step(0.5, uv.y);
                float bangs = pow(abs(uv.x*2-1), _BangRoundness) + pow(abs(uv.y*2-1), _BangRoundness);
                bangs = (1-step(1, bangs)) * step(0.5, 1-uv.y);
                bang+=bangs;
                clip(bang-0.5);
                float uvCol = i.uv.y;
                uvCol = cos(uvCol*(3.14*2)*pow(uvCol,2));
                uvCol *= 0.5;
                uvCol += 0.5;
                
                float4 col = lerp(_Color1, _Color2, 1-uvCol);
                return col;
            }
            ENDCG
        }
    }
}
