Shader "Unlit/HeadQuad"
{
    Properties
    {

        [HideInInspector]_Radius("Radius", Range(.25, 10)) = 4
        [HideInInspector]_Radius2("Radius 2", Range(1, 10)) = 4
        _Radius3("Jaw WIdth", Range(.1, 5)) = 5
        _Radius4("Jaw Height", Range(.1, 5)) = 5
        _Radius5("Head Top Width", Range(.1, 5)) = 5
        _Radius6("Head Height", Range(.1, 5)) = 5
        _Color("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
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
            float4 _Color, _Color2;
            float _Radius, _Radius2, _Radius3, _Radius4, _Radius5, _Radius6;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z/2+3)/20, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                float value = pow(abs(uv.x*2-1), _Radius5) + pow(abs(uv.y*2-1), _Radius6);
                float value2 = pow(abs(uv.x*2-1), _Radius3) + pow(abs(uv.y*2-1), _Radius4);
                //float value3 = step(1, pow(abs(uv.x*2-1), _Radius3) + pow(abs(uv.y*2-1), _Radius3))* (1-step(0.5, i.uv.y));
                //value3 = 1 - value3;
                value = step(1, value) * step(0.5, i.uv.y);
                value2 = step(1, value2) * (1-step(0.5, i.uv.y));
                value += value2;
                value = 1 - value;
                clip(value - 0.5);
                
                float4 result = value * lerp(_Color, _Color2, uv.y);
                return result;//lerp(float4(1, 0, 0, 1), float4(1,1,1,1), 1-uv.y);
            }
            ENDCG
        }
    }
}
