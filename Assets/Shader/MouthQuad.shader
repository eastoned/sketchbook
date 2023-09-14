Shader "Unlit/MouthQuad"
{
    Properties
    {

        _Radius ("Radius", Range(0.1, 1)) = 0.5
        
        _xScaleUpper ("x SCale", Range(0, 1)) = 0.5
        
        _yScaleUpper ("yScale", Range(-1, 1)) = 0.5
        
        _yScaleUpper2 ("yScale2", Range(-1, 1)) = 0.5

        _TopTeeth ("Teeththop", Range(0,1)) = 0.5
        _BotTeeth ("Teeth botoom", Range(0,1)) = 0.5

        _TongueScale("TongueScale", Range(0.25, 0.75)) = 0.5
        _TongueRadius("Tongue Size", Range(0.1, 0.5)) = 0.5
        _TongueHeight("Tongue Height", Range(0, 0.5)) = 0

        _TeethAmount("AmountTeeth", Range(0, 30)) = 4
        _TeethRoundness("Roundness", Range(0.5, 4)) = 1

        _Color("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
        _Color3("Color3", Color) = (1,1,1,1)
        _Color4("Color4", Color) = (1,1,1,1)
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
            float _Radius, _xScaleUpper, _yScaleUpper;
            float _Radius2, _xScaleUpper2, _yScaleUpper2;
            float _TopTeeth, _BotTeeth;
            float _TongueRadius, _TongueScale, _TongueHeight;
            float4 _Color, _Color2, _Color3, _Color4;
            int _TeethAmount;
            float _TeethRoundness;

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
                float line1 = step(0, (pow(_Radius/2, 2) - pow((uv.x-0.5), 2)) - pow((uv.y-0.5)/_yScaleUpper,2)) * step(0.5, uv.y);
                float line2 = step(0, (pow(_Radius/2, 2) - pow((uv.x-0.5), 2)) - pow((uv.y-0.5)/_yScaleUpper2,2)) * (1 - step(0.5, uv.y));
                //min of two scale values absolute 
                //if upper and upper 2 are <0 then mouth is gone, how do we address
                float mask = 1-clamp(-min(_yScaleUpper, _yScaleUpper2), 0, 1);
                float line3 = 1-step(0, (pow(_Radius/2, 2) - pow((uv.x-0.5)*_xScaleUpper, 2)) - pow((uv.y-0.5)/clamp(-min(_yScaleUpper, _yScaleUpper2), 0, 1),2));
                line1 += line2;
                line1 *= line3;
                float uvStrength = ((-_yScaleUpper + 3) * (-_yScaleUpper2 + 3));
                float2 teethUV = float2(frac(uv.x*_TeethAmount), (uv.y*(uvStrength)-(uvStrength/2) - _yScaleUpper + _yScaleUpper2));//* (_yScaleUpper*-1 + 2);// ;
                //teethUV = uv.y + (((_yScaleUpper*0.5)+0.5)*-.25 -.25) * (((_yScaleUpper2*0.5)+0.5)*.25-.75);
                //teethUV += (((_yScaleUpper2*0.5)+0.5)*.25-.75);
                float topComp = teethUV.y -(1-_TopTeeth)*2 - 0.5;
                float line4 = step(0, topComp);
                float botComp = -teethUV.y - (1-_BotTeeth)*2 - 0.5;
                line4 += step(0, botComp);
                
                float toof = pow(abs(teethUV.x*2-1), _TeethRoundness) + pow(abs(topComp*2), _TeethRoundness);
                float toof2 = pow(abs(teethUV.x*2-1), _TeethRoundness) + pow(abs(botComp*2), _TeethRoundness);
                toof = 1-step(1, toof);
                toof2 = 1 - step(1, toof2);
                toof += toof2;
                
                line4 += toof;
                float tongue = step(0, pow(_TongueRadius, 2) - pow((uv.x-.5)*(_TongueScale)*2, 2) - pow((uv.y-_TongueHeight)*(1-_TongueScale)*2,2));
                float4 tonColor = tongue * lerp(_Color3, _Color4, ((uv.y-_TongueHeight)*_TongueScale));
                float4 mouthColor = (1-tongue) * lerp(_Color, _Color2, uv.y);
                
                clip(line1.r-0.5);
                //line4 = saturate(line4+line5);
                float4 res = saturate(saturate(tonColor + mouthColor) + line4);
                return res;
            }
            ENDCG
        }
    }
}
