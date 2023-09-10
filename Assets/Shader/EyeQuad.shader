Shader "Unlit/EyeQuad"
{
    Properties
    {
        _Radius ("Radius", Range(0, 1)) = 0.5
        _ColorTop("Top Color", Color) = (0,0,0,0)
        _ColorBottom("Bottom Color", Color) = (0,0,0,0)
        _xScaleUpper ("X Scale Upper", Range(0,1)) = 1
        _yScaleUpper ("Y Scale Upper", Range(0,1)) = 1
        _xScaleLower ("X Scale Lower", Range(0,1)) = 1
        _yScaleLower ("Y Scale Lower", Range(0,1)) = 1
        _xPupil ("X Scale Pupil", Range(0, 1)) = 1
        _pupilOffsetX ("Translate pupil x", Range(-1, 1)) = 0
        _pupilOffsetY ("Translate pupil y", Range(-1, 1)) = 0
        _yPupil ("Y Scale Pupil", Range(0, 1)) = 1
        _radiusPupil ("Pupil Radius", Range(0,1)) = 1

        _xLevel ("X Level", Range(-5, 5)) = 1
        _yLevel ("Y Level", Range(0, 2)) = 1
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

            float _Radius, _xScaleUpper, _yScaleUpper, _xScaleLower, _yScaleLower, _radiusPupil, _xPupil, _yPupil, _pupilOffsetX, _pupilOffsetY;
            float4 _ColorTop, _ColorBottom;
            float _yLevel, _xLevel;

            float random (float2 st) {
                return frac(sin(dot(st.xy,
                                    float2(12.9898,78.233)))*
                    43758.5453123);
            }

            v2f vert (appdata v)
            {
                v2f o;
                //v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z*2)/8, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = float2(i.uv.x, i.uv.y);
                float line1 = step(0, (pow(_Radius/2, 2) - pow((uv.x-0.5)/_xScaleUpper, 2)) - pow((uv.y-0.5)/_yScaleUpper,2)) * step(0.5, i.uv.y);
                float line2 = step(0, (pow(_Radius/2, 2) - pow((uv.x-0.5)/_xScaleLower, 2)) - pow((uv.y-0.5)/_yScaleLower,2)) * (1 - step(0.5, i.uv.y));
                float4 result = (line1 * _ColorTop) + (line2 * _ColorBottom);
                clip(result.a - 0.5);
                float pupil = 1 - step(0, (pow(_radiusPupil/2*_Radius, 2) - pow((uv.x-0.5 + _pupilOffsetX)/_xPupil, 2)) - pow((uv.y-0.5 +_pupilOffsetY)/_yPupil,2));
                
                result *= pupil;
                return result;
            }
            ENDCG
        }
    }
}
