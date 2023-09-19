Shader "Unlit/NoseQuad"
{
    Properties
    {
        _NoseBaseWidth ("Nose Base Width", Range(0.1,2.5)) = 1
        _NoseTotalWidth ("Nose Total Width", Range(1,5)) = 1
        _NoseTopWidth ("Nose Top Width", Range(0.1,2.5)) = 1
        _NoseCurve ("Nose Curve", Range(1,5)) = 1
        _NoseTotalLength("Nose Total Length", Range(0.1, 3)) = 0
        _NostrilRadius("Nostril Radius", Range(0, 0.5)) = 0.25
        _NostrilSpacing("Nostril Spacing", Range(0, 1)) = 0.5
        _NostrilHeight("Nostril Height", Range(-.5, 0)) = 0
        _NostrilScale("Nostril Scale", Range(0.25, 2)) = 0.5

        _Color1("Bottom", Color) = (1,1,1,1)
        _Color2("Top", Color) = (1,1,1,1)
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

            float _NoseTotalWidth, _NoseBaseWidth;
            float _NoseCurve, _NoseTopWidth;
            float _NoseTotalLength, _yOffset, _NostrilRadius, _NostrilSpacing, _NostrilHeight, _NostrilScale;
            float4 _Color1, _Color2;
            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-1.5)/60, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float2 uv = float2(abs(i.uv.x*2-1), lerp(i.uv.y, i.uv.y - 0.35, _NoseTotalLength/3) * _NoseTotalLength);
                float line1 = pow(uv.y, _NoseBaseWidth) - uv.x * _NoseTotalWidth;

                float line2 = pow(1-uv.y, _NoseTopWidth) - uv.x * _NoseCurve;
                //* _NostrilRadius
                //float circle1 = step(_NostrilRadius, distance(uv*float2(_NostrilScale,1), float2(_NostrilSpacing*_NostrilRadius, 0.5+_NostrilHeight)));
                float circle1 = step(_NostrilRadius, distance(uv*float2(_NostrilScale, 1), float2(_NostrilSpacing*_NostrilScale, _NostrilHeight+0.5)));
                
                float result = step(0, line1*line2);
                
                result = lerp(line1, line2, uv.y);
                result = step(0, result);
                result *= circle1;
                clip(result.r - 0.5);
                float4 final = result * lerp(_Color1, _Color2, saturate(max(i.uv.y, uv.y)));
                return final;
            }
            ENDCG
        }
    }
}
