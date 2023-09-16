Shader "Unlit/EyeQuad"
{
    Properties
    {
        _Radius ("Radius", Range(0, 1)) = 0.5
        
        _xPupil ("X Scale Pupil", Range(0.1, 1)) = 1
        _pupilOffsetX ("Translate pupil x", Range(-1, 1)) = 0
        _pupilOffsetY ("Translate pupil y", Range(-1, 1)) = 0
        _yPupil ("Y Scale Pupil", Range(0.1, 1)) = 1
        _radiusPupil ("Pupil Radius", Range(0,1)) = 1

        _xLevel ("X Level", Range(0, 2.75)) = 1
        _yLevel ("Y Level", Range(0, 1)) = 1

        _xLevel3 ("X Level3", Range(0, 2.75)) = 1
        _yLevel3 ("Y Level3", Range(0, 1)) = 1

        _Lid1 ("Lid 1", Range(0, 1)) = 0.5
        _Lid2 ("Lid 2", Range(0, 1)) = 0.5

        _SquashPupil ("Squash", Range(0.25, 1)) = 1
        
        _Color1("Top Color", Color) = (0,0,0,0)
        _Color2("Bottom Color", Color) = (0,0,0,0)
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
            float4 _Color1, _Color2;
            float _yLevel, _xLevel, _yLevel3, _xLevel3;

            float _Lid1, _Lid2;
            float _SquashPupil;

            float random (float2 st) {
                return frac(sin(dot(st.xy,
                                    float2(12.9898,78.233)))*
                    43758.5453123);
            }

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-1)/30, v.vertex.z, v.vertex.w);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = float2(i.uv.x, i.uv.y);
                float line1 = step(0, (pow(abs(_Radius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _xLevel*1.5*((_yLevel*uv.x)+((1-_yLevel)*(1-uv.x))))) 
                * step(0.5, uv.y);

                float line2 = step(0, (pow(abs(_Radius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _xLevel3*1.5*((_yLevel3*uv.x)+((1-_yLevel3)*(1-uv.x))))) 
                * (1 - step(0.5, uv.y));

                float mask1 = step(0, (pow(abs(_Radius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _xLevel*_Lid1*1.5*((_yLevel*uv.x)+((1-_yLevel)*(1-uv.x))))) 
                * step(0.5, uv.y);

                float mask2 = step(0, (pow(abs(_Radius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _xLevel3*_Lid2*1.5*((_yLevel3*uv.x)+((1-_yLevel3)*(1-uv.x))))) 
                * (1 - step(0.5, uv.y));

                mask1 += mask2;

                float4 result = line1 + line2;
                clip(result.a - 0.5);
                float pupil = 1 - step(0, (pow(_radiusPupil/2*_Radius, 2*_SquashPupil) - pow(abs((uv.x-0.5 + _pupilOffsetX)/_xPupil), 2*_SquashPupil)) - pow(abs((uv.y-0.5 +_pupilOffsetY)/_yPupil),2*_SquashPupil));
                
                result *= pupil;
                result *= mask1;
                mask1 = 1 - mask1;
                
                float4 shades = lerp(_Color1, _Color2, abs(uv.x*2-1)) * mask1;
                result += shades;
                return result;
            }
            ENDCG
        }
    }
}
