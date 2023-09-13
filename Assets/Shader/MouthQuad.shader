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

            v2f vert (appdata v)
            {
                v2f o;
               /// v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z+2)/10, v.vertex.z, v.vertex.w);
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
                float mask = 1-clamp(-min(_yScaleUpper, _yScaleUpper2), 0, 1);
                float line3 = 1-step(0, (pow(_Radius/2, 2) - pow((uv.x-0.5)*_xScaleUpper, 2)) - pow((uv.y-0.5)/clamp(-min(_yScaleUpper, _yScaleUpper2), 0, 1),2));
                line1 += line2;
                line1 *= line3;

                //-1 to 1 is 0 to 1
                float line4 = step(_yScaleUpper/2 + 0.5 - _TopTeeth*mask/2, uv.y) * step(0.25, uv.y);
                float line5 = step(_yScaleUpper2/2 + 0.5 - _BotTeeth*mask/2, 1-uv.y) * (step(0.25, 1-uv.y));
                clip(line1.r-0.5);
                float4 res = float4(0,0,0,1) * line1 + saturate(line4 + line5);
                return res;
            }
            ENDCG
        }
    }
}
