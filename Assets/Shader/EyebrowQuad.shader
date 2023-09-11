Shader "Unlit/EyebrowQuad"
{
    Properties
    {
        _Strokes ("Strokes", Range(1, 16)) = 1
        _Offset ("Offset", Range(1, 8)) = 0
        _Curve ("Curve", Range(0.3, 4)) = 1

        _Curve2 ("Curve2", Range(-1, 1)) = 0
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

            int _Strokes;
            float _Offset;
            float _Curve;
            float _Curve2;

            v2f vert (appdata v)
            {
                v2f o;
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
                
                uv *= float2(_Strokes, 1);
                uv = frac(uv);
                uv += float2(0, stretchUV * _Curve2 * (_Offset-1)/16);

                float result = pow(abs(uv.x*2-1), _Curve) + pow(abs(uv.y*2-1)*_Offset, _Curve);
                result = step(1, result);
                //float result = sin(uv.x * _Strokes * 3.14 + _Offset)/6 - uv.y + .5;
                //float result2 = -sin(uv.x * _Strokes * 3.14 + _Offset)/6 - uv.y + 0.5;
                //float result2 = sin(
                //float final = step(0, max(result2, result)*1-step(0, min(result2, result)));
                //clip(final - 0.5);
                //final *= 0;
                clip(1-result-0.5);
                return result.xxxx;
            }
            ENDCG
        }
    }
}
