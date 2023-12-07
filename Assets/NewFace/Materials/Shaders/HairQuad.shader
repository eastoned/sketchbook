Shader "Unlit/HairQuad"
{
    Properties
    {
        _BangRoundness ("Band Roundness", Range(0, 1)) = 1
        _StrandCount ("Strand Count", Range(0, 1)) = 0
        _StrandOffset ("Strand Offset", Range(0, 1)) = 0
        _HairBangScale ("Hair Bang Scale", Range(0, 1)) = 0
        _HairRoundness ("Hair Roundness", Range(0, 1)) = 1

        _Color1("Base", Color) = (1,1,1,1)
        _Color2("Accent", Color) = (0.5,0.5,0.5, 1)

        _MainTex("Tex", 2D) = "white" {}
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
                float4 screenPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _BangRoundness, _HairBangScale;
            float _StrandCount;
            float _StrandOffset;
            float _HairRoundness;
            float4 _Color1, _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z)/60, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                uv.x *= (int)(19*_StrandCount+1);
                uv.x += (0.5*_StrandOffset);
                uv.x = frac(uv.x);
                uv.y = pow(uv.y, (1.5*_HairBangScale+0.5));
                
                float bang = pow(abs(i.uv.x*2-1), (4*_HairRoundness+1)) + pow(abs(uv.y*2-1), (4*_HairRoundness+1));
                bang = (1-step(1, bang)) * step(0.5, uv.y);
                float bangs = pow(abs(uv.x*2-1), (3.75*_BangRoundness+.25)) + pow(abs(uv.y*2-1), (3.75*_BangRoundness+.25));
                bangs = (1-step(1, bangs)) * step(0.5, 1-uv.y);
                bang+=bangs;
                bang = saturate(bang);
                //bang *= step(0.00001,1-abs(uv.y*2 - 1));
                clip(bang-0.5);
                float uvCol = i.uv.y;
                uvCol = cos(uvCol*(3.14*2)*pow(uvCol,2));
                uvCol *= 0.5;
                uvCol += 0.5;
                
                

                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);
                float4 col2 = tex2D(_MainTex, texCoord);
                float4 col = lerp(_Color1, _Color2, 1-uvCol);
                //col = lerp(_Color1, _Color2, 1-step(1-uvCol/2,col2));
                return col*col2;
            }
            ENDCG
        }
    }
}
