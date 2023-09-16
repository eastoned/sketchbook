Shader "Unlit/NeckQuad"
{
    Properties
    {
        _NeckTopWidth ("Neck Width", Range(1, 5)) = 0
        _NeckCurveRoundness("Neck Radius", Range(0, 3)) = 1
        _NeckCurveScale("Neck Scale", Range(1, 5)) = 0.5

        _Color1("Top", Color) = (1,1,1,1)
        _Color2("Bottom", Color) = (1,1,1,1)
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

            float _NeckTopWidth, _NeckCurveRoundness, _NeckCurveScale;
            float4 _Color1, _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float2 uv = i.uv;//*float2(2, 2) - float2(0.5, 0.5);
                //
                float line1 = step(0, pow(0.5, 2) - pow(abs(uv.x), _NeckTopWidth*2) - pow(abs(uv.y-0.5),2));
                float line2 = step(0, (pow(0.5, 2) - pow(abs((uv.x-1))-(uv.y*_NeckCurveScale), _NeckTopWidth*2)) - pow(abs(uv.y-0.5),2));
                float lin3 = pow((uv.x * _NeckTopWidth*2) , _NeckCurveRoundness) + pow(1-abs(uv.y), _NeckCurveScale);
                float lin4 = pow(((1-uv.x) * _NeckTopWidth*2) , _NeckCurveRoundness) + pow(1-abs(uv.y), _NeckCurveScale);
                
                lin3 = step(1, lin3) * 1 - step(0.5, i.uv.x);
                lin4 = step(1, lin4) * step(0.5, i.uv.x);
                lin4 += lin3;
                clip(lin4 - 0.5);

                return lerp(_Color1, _Color2, 1-uv.y);;
            }
            ENDCG
        }
    }
}
