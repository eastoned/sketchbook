Shader "Unlit/HairQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BangRoundness ("Band Roundness", Range(0.25, 4)) = 1
        _BangCount ("Bang Count", Range(0, 20)) = 2
        _BangOffset ("Bang Offset", Range(0, 1)) = 0
        _yOffset("Y Offset", Range(0.5, 2)) = 0
        _TopRoundness("Top Roundness", Range(2, 4)) = 2
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
            float _BangRoundness, _yOffset;
            int _BangCount;
            int _BangOffset;
            float _TopRoundness;

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
                
                uv.x *= _BangCount;
                uv.x += (0.5*_BangOffset);
                uv = frac(uv);
                uv.y = pow(uv.y, _yOffset);
                
                float bang = pow(abs(i.uv.x*2-1), _TopRoundness) + pow(abs(uv.y*2-1), _TopRoundness);
                bang = (1-step(1, bang)) * step(0.5, uv.y);
                float bangs = pow(abs(uv.x*2-1), _BangRoundness) + pow(abs(uv.y*2-1), _BangRoundness);
                bangs = (1-step(1, bangs)) * step(0.5, 1-uv.y);
                bang+=bangs;
                clip(bang-0.5);
                return uv.yyyy;
            }
            ENDCG
        }
    }
}
