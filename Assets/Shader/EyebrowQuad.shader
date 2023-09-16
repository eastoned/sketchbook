Shader "Unlit/EyebrowQuad"
{
    Properties
    {
        _EyebrowCount ("Eyebrow Count", Range(1, 16)) = 1
        _EyebrowThickness ("Eyebrow Thickness", Range(1, 8)) = 0
        _EyebrowRoundness ("Eyebrow Roundness", Range(0.3, 4)) = 1

        _EyebrowCurve ("Eyebrow Curve", Range(-1, 1)) = 0

        _Color1 ("Inner", Color) = (1,1,1,1)
        _Color2 ("Outer", Color) = (1,1,1,1)
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

            int _EyebrowCount;
            float _EyebrowThickness;
            float _EyebrowRoundness;
            float _EyebrowCurve;
            float4 _Color1, _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-1.5)/20, v.vertex.z, v.vertex.w);

                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z+2)/10, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float stretchUV = sin(uv.x*3.14);
                
                uv *= float2(_EyebrowCount, 1);
                uv = frac(uv);
                uv += float2(0, stretchUV * _EyebrowCurve * (_EyebrowThickness-1)/16);

                float result = pow(abs(uv.x*2-1), _EyebrowRoundness) + pow(abs(uv.y*2-1)*_EyebrowThickness, _EyebrowRoundness);
                result = 1-step(1, result);
                clip(result-0.5);
                float4 eyebrowCol = result * lerp(_Color1, _Color2, i.uv.x);
                //float result = sin(uv.x * _EyebrowCount * 3.14 + _EyebrowThickness)/6 - uv.y + .5;
                //float result2 = -sin(uv.x * _EyebrowCount * 3.14 + _EyebrowThickness)/6 - uv.y + 0.5;
                //float result2 = sin(
                //float final = step(0, max(result2, result)*1-step(0, min(result2, result)));
                //clip(final - 0.5);
                //final *= 0;
                
                return eyebrowCol;
            }
            ENDCG
        }
    }
}
