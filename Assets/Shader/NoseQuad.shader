Shader "Unlit/NoseQuad"
{
    Properties
    {
        _xScaleUpper ("X Scale Upper", Range(0.1,5)) = 1
        _yScaleUpper ("Y Scale Upper", Range(1,10)) = 1
        _xScaleUpper2 ("X Scale Upper2", Range(0.1,5)) = 1
        _yScaleUpper2 ("Y Scale Upper2", Range(1,10)) = 1
        _Blend("Blend", Range(0.1, 2)) = 0
        _Radius("Nostril Radius", Range(0.1, 1)) = 0.5
        _NostrilSpacing("NostrilSpace", Range(1, 2)) = 0.5
        _NostrilHeight("Nostril Height", Range(-.5, 0)) = 0
        _NostrilScale("Nostril Scale", Range(0, 2)) = 0.5
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

            float _yScaleUpper, _xScaleUpper;
            float _yScaleUpper2, _xScaleUpper2;
            float _Blend, _yOffset, _Radius, _NostrilSpacing, _NostrilHeight, _NostrilScale;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z+1)/10, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float2 uv = float2(abs(i.uv.x*2-1), i.uv.y * _Blend);
                float line1 = pow(uv.y, _xScaleUpper) - uv.x * _yScaleUpper;

                float line2 = pow(1-uv.y, _xScaleUpper2) - uv.x * _yScaleUpper2;

                float circle1 = step(_Radius, distance(uv*float2(_NostrilScale,1), float2(_NostrilSpacing * _Radius, 0.5+_NostrilHeight)));
                
                float result = step(0, line1*line2);
                
                result = lerp(line1, line2, uv.y);
                result = step(0, result);
                result *= circle1;
                clip(result.r - 0.5);
                float4 final = result * lerp(float4(1,0,0,1), float4(1,1,1,1), uv.y);
                return final;
            }
            ENDCG
        }
    }
}
