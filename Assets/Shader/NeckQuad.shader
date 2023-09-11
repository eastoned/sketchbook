Shader "Unlit/NeckQuad"
{
    Properties
    {
        _Width ("Width", Range(0, 5)) = 0
        _Radius("Radius", Range(0.5, 15)) = 1
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

            float _Width, _Radius, _Radius2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            //width is .2, radius can be 15
            //width is 15, radius has to be .5
                float2 uv = i.uv;
                float line1 = step(0, (pow(0.5, 2) - pow(uv.x/_Radius, 2*_Width*1/_Radius)) - pow(uv.y-0.5,2));
                float line2 = step(0, (pow(0.5, 2) - pow(abs((uv.x-1))/_Radius, 2*_Width*1/_Radius)) - pow(uv.y-0.5,2));

                line2 += line1;
                line2 = saturate(line2);
                line2 = 1 - line2;
                clip(line2 - 1);
                return lerp(float4(1,0,0,1), float4(1,0.5,1,1), 1-uv.y);;
            }
            ENDCG
        }
    }
}
