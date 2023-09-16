Shader "Unlit/EarQuad"
{
    Properties
    {
        
        _EarWidthSkew("Ear Width Skew", Range(0, 1)) = 0
        _EarLengthSkew("Ear Length Skew", Range(-1, 1)) = 0
        _EarShape("Ear Shape", Range(1, 6)) = 0
        _EarRoundness("Ear Roundness", Range(0.6, 1.25)) = 0


        _EarOpenWidth("Ear Open Width", Range(1, 1.5)) = 0
        _EarOpenLength("Ear Open Length", Range(1, 1.5)) = 0

        _EarConcha ("Ear Concha", Range(0.5, 1.25)) = 0
        _EarTragus("Ear Tragus", Range(0, 1)) = 0.5

        _Color1("Bottom", Color) = (1,1,1,1)
        _Color2("Top", Color) = (1,1,1,1)
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

      
            float _EarWidthSkew, _EarLengthSkew, _EarShape, _EarRoundness, _EarOpenWidth, _EarOpenLength, _EarConcha, _EarTragus;
            float4 _Color1, _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-0.5)/40, v.vertex.z, v.vertex.w);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv*2*float2(0.5, 1) + float2(0, -1);
            
                float2 warp = uv;
                float res = pow(uv.x*_EarWidthSkew, 3*_EarRoundness) - pow(uv.y*_EarLengthSkew, 3) - pow(pow(uv.x*_EarOpenWidth, 2) + pow(uv.y*_EarOpenLength, 2), 2*_EarShape);
                res = step(0, res);

                clip(res-0.5);
                float line1 = step(0.5, distance(float2(0.5, -_EarLengthSkew/5 + lerp(0, 0.2, _EarLengthSkew*0.5+0.5)), i.uv*float2(_EarConcha, _EarConcha)));

                float line2 = 1-step(_EarTragus*lerp(1, 0.5, _EarLengthSkew*0.5+0.5), distance(float2(0, -_EarLengthSkew*0.5+0.5), i.uv));
                line1 = saturate(line1 + line2);
                float4 ear = line1 * lerp(_Color1, _Color2, i.uv.y);
                return ear;
            }
            ENDCG
        }
    }
}
