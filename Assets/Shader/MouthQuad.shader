Shader "Unlit/MouthQuad"
{
    Properties
    {

        _MouthRadius ("Mouth Radius", Range(0.1, 1)) = 0.5
        
        _MouthLipMaskRoundness ("Lip Mask Roundness", Range(0, 1)) = 0.5
        
        _MouthLipTop ("Top Lip", Range(-1, 1)) = 0.5
        
        _MouthLipBottom ("Bottom Lip", Range(-1, 1)) = 0.5

        _TeethTop ("Top Teeth", Range(0,1)) = 0.5
        _TeethBottom ("Bottom Teeth", Range(0,1)) = 0.5
        _TeethCount("Teeth Count", Range(0, 30)) = 4
        _TeethRoundness("Teeth Roundness", Range(0.5, 4)) = 1

        _TongueRadius("Tongue Radius", Range(0, 0.25)) = 0.25
        _TongueScale("Tongue Scale", Range(0.25, 0.75)) = 0.5
        _TongueHeight("Tongue Height", Range(0, 1)) = 0

        _Lips("Lips", Range(0, 4)) = 0.5

        _Color1("Mouth Bottom", Color) = (1,1,1,1)
        _Color2("Mouth Top", Color) = (1,1,1,1)
        _Color3("Tongue Bottom", Color) = (1,1,1,1)
        _Color4("Tongue Top", Color) = (1,1,1,1)
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

            float _MouthRadius, _MouthLipMaskRoundness, _MouthLipTop;
            float _MouthRadius2, _MouthLipMaskRoundness2, _MouthLipBottom;
            float _TeethTop, _TeethBottom;
            float _TongueRadius, _TongueScale, _TongueHeight;
            float4 _Color1, _Color2, _Color3, _Color4;
            int _TeethCount;
            float _TeethRoundness;
            float _Lips;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-2)/60, v.vertex.z, v.vertex.w);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float value = distance(i.uv, float2(0.5, 0.5));
                value = step(0.5, value);
                float line0 = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5), 2)) - pow((uv.y-0.5)/_MouthLipTop,2);
                float line1 = step(0, line0) * step(0.5, uv.y);
                line0 = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5), 2)) - pow(abs((uv.y-0.5)/_MouthLipTop), 2);
                line0 *= step(0.5, uv.y);

                float line2a = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5), 2)) - pow((uv.y-0.5)/_MouthLipBottom,2);
                float line2 = step(0, line2a) * (1 - step(0.5, uv.y));
                line2a = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5), 2)) - pow(abs((uv.y-0.5)/_MouthLipBottom),2);
                line2a *= (1 - step(0.5, uv.y));
                //min of two scale values absolute 
                //if upper and upper 2 are <0 then mouth is gone, how do we address
                float mask = 1-clamp(-min(_MouthLipTop, _MouthLipBottom), 0, 1);
                float line3a = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5)*_MouthLipMaskRoundness, 2)) - pow((uv.y-0.5)/clamp(-min(_MouthLipTop, _MouthLipBottom), 0, 1),2);
                float line3 = 1-step(0, line3a);
                line3a = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5)*_MouthLipMaskRoundness, 2)) - pow(abs(0.5*(uv.y-0.5)/clamp(-min(_MouthLipTop, _MouthLipBottom), 0, 1)),2);
                //line3a = 1 - step(0, line3a);
                line1 += line2;
                line1 *= line3;

               

                //line0 += line2a;
                //line0 *= line3a;
                float uvStrength = ((-_MouthLipTop + 3) * (-_MouthLipBottom + 3));
                float2 teethUV = float2(frac(uv.x*_TeethCount), (uv.y*(uvStrength)-(uvStrength/2) - _MouthLipTop + _MouthLipBottom));//* (_MouthLipTop*-1 + 2);// ;
                //teethUV = uv.y + (((_MouthLipTop*0.5)+0.5)*-.25 -.25) * (((_MouthLipBottom*0.5)+0.5)*.25-.75);
                //teethUV += (((_MouthLipBottom*0.5)+0.5)*.25-.75);
                float topComp = teethUV.y -(1-_TeethTop)*2 - 0.5;
                float line4 = step(0, topComp);
                float botComp = -teethUV.y - (1-_TeethBottom)*2 - 0.5;
                line4 += step(0, botComp);
                
                float toof = pow(abs(teethUV.x*2-1), _TeethRoundness) + pow(abs(topComp*2), _TeethRoundness);
                float toof2 = pow(abs(teethUV.x*2-1), _TeethRoundness) + pow(abs(botComp*2), _TeethRoundness);
                toof = 1-step(1, toof);
                toof2 = 1 - step(1, toof2);
                toof += toof2;
                
                line4 += toof;
                float tongue = step(0, pow(_TongueRadius, 2) - pow((uv.x-.5) *(1-_TongueScale), 2) - pow((uv.y-_TongueHeight)*(_TongueScale),2));
                float4 tonColor = tongue * lerp(_Color3, _Color4,((_TongueScale*uv.y*4)-_TongueHeight/2 + .25));//lerp(_Color3, _Color4, ((uv.y-_TongueHeight)*_TongueScale));
                float4 mouthColor = (1-tongue) * lerp(_Color1, _Color2, uv.y);
                
                //float lipOutline;
                //line1 += _Lips;

                clip(line1.r-0.5);
                //line4 = saturate(line4+line5);
                float4 res = saturate(saturate(tonColor + mouthColor) + line4);
                
                return res * pow(_MouthLipTop + _MouthLipBottom, 0.5);
            }
            ENDCG
        }
    }
}
