Shader "Unlit/EyeQuad"
{
    Properties
    {
        _EyeRadius ("Eye Radius", Range(0, 1)) = 0.5
        _PupilRadius ("Pupil Radius", Range(0,1)) = 1

        _PupilWidth ("Pupil Width", Range(0.1, 1)) = 1
        _PupilLength ("Pupil Length", Range(0.1, 1)) = 1
        _PupilOffsetX ("Pupil Offset X", Range(-1, 1)) = 0
        _PupilOffsetY ("Pupil Offset Y", Range(-1, 1)) = 0
        
        

        _EyelidTopLength ("Eyelid Top Length", Range(0, 2.75)) = 1
        _EyelidTopSkew ("Eyelid Top Skew", Range(0, 1)) = 1

        _EyelidBottomLength ("Eyelid Bottom Length", Range(0, 2.75)) = 1
        _EyelidBottomSkew ("Eyelid Bottom Skew", Range(0, 1)) = 1

        _EyelidTopOpen ("Eyelid Top Open", Range(0, 1)) = 0.5
        _EyelidBottomOpen ("Eyelid Bottom Open", Range(0, 1)) = 0.5

        _PupilRoundness ("Pupil Roundness", Range(0.25, 1)) = 1
        
        _Color1("Eyelid Center", Color) = (0,0,0,0)
        _Color2("Eyelid Edge", Color) = (0,0,0,0)

        _Color3("Pupil", Color) = (0,0,0,1)
        _Color4("Iris", Color) = (0,0,0,1)

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

            float _EyeRadius, _PupilRadius, _PupilWidth, _PupilLength, _PupilOffsetX, _PupilOffsetY;
            float4 _Color1, _Color2;
            float _EyelidTopSkew, _EyelidTopLength, _EyelidBottomSkew, _EyelidBottomLength;
            
            float _EyelidTopOpen, _EyelidBottomOpen;
            float _PupilRoundness;

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
                float line1 = step(0, (pow(abs(_EyeRadius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _EyelidTopLength*1.5*((_EyelidTopSkew*uv.x)+((1-_EyelidTopSkew)*(1-uv.x))))) 
                * step(0.5, uv.y);

                float line2 = step(0, (pow(abs(_EyeRadius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _EyelidBottomLength*1.5*((_EyelidBottomSkew*uv.x)+((1-_EyelidBottomSkew)*(1-uv.x))))) 
                * (1 - step(0.5, uv.y));

                float mask1 = step(0, (pow(abs(_EyeRadius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _EyelidTopLength*_EyelidTopOpen*1.5*((_EyelidTopSkew*uv.x)+((1-_EyelidTopSkew)*(1-uv.x))))) 
                * step(0.5, uv.y);

                float mask2 = step(0, (pow(abs(_EyeRadius/2), 2) - pow(abs(uv.x-0.5)/1, 2)) - pow(abs(uv.y-0.5)/1,
                _EyelidBottomLength*_EyelidBottomOpen*1.5*((_EyelidBottomSkew*uv.x)+((1-_EyelidBottomSkew)*(1-uv.x))))) 
                * (1 - step(0.5, uv.y));

                mask1 += mask2;

                float4 result = line1 + line2;
                clip(result.a - 0.5);
                float pupil = 1 - step(0, (pow(_PupilRadius/2*_EyeRadius, 2*_PupilRoundness) - pow(abs((uv.x-0.5 + _PupilOffsetX)/_PupilWidth), 2*_PupilRoundness)) - pow(abs((uv.y-0.5 +_PupilOffsetY)/_PupilLength),2*_PupilRoundness));
                
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
