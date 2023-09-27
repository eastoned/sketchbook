Shader "Unlit/EarQuad"
{
    Properties
    {
        
        _EarWidthSkew("Ear Width Skew", Range(0, 1)) = 0
        _EarLengthSkew("Ear Length Skew", Range(0, 1)) = 0
        _EarShape("Ear Shape", Range(0, 1)) = 0
        _EarRoundness("Ear Roundness", Range(0, 1)) = 0


        _EarOpenWidth("Ear Open Width", Range(0, 1)) = 0
        _EarOpenLength("Ear Open Length", Range(0, 1)) = 0

        _EarConcha ("Ear Concha", Range(0, 1)) = 0
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
                v.vertex = lerp(v.vertex, float4(v.vertex.x, v.vertex.y + sin(_Time.z-0.5)/40, v.vertex.z, v.vertex.w), v.uv.x);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv*2*float2(0.5, 1) + float2(0, -1);
                
                float2 warp = uv;
                float res = pow(uv.x*_EarWidthSkew, 3*(.65*_EarRoundness+.6)) - pow(uv.y*(2*_EarLengthSkew-1), 3) - pow(pow(uv.x*(.5*_EarOpenWidth+1), 2) + pow(uv.y*(.5*_EarOpenLength+1), 2), 2*(5*_EarShape+1));
                res = step(0, res);

                clip(res-0.5);
                float line1 = step(0.5, distance(float2(0.5, -(2*_EarLengthSkew-1)/5 + lerp(0, 0.2, _EarLengthSkew)), i.uv*float2((.75*_EarConcha+.5), (.75*_EarConcha+.5))));

                //float line2 = 1-step(_EarTragus*lerp(2, 1, _EarLengthSkew), distance(float2(0, -_EarLengthSkew), i.uv));
                float line2 = 1-step(_EarTragus, distance(float2(0, lerp(0.5, 0,_EarLengthSkew)), i.uv));
                line1 = saturate(line1 + line2);
                float4 ear = line1 * lerp(_Color1, _Color2, i.uv.y);
                return ear;
            }
            ENDCG
        }
    }
}
