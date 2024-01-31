Shader "Unlit/HeadQuad"
{
    Properties
    {
        _ChinWidth("Chin Width", Range(0, 1)) = 1
        _ChinLength("Chin Length", Range(0, 1)) = 1
        _ChinScale ("Chin Scale", Range(0,1)) = 1
        _ForeheadWidth("Forehead Width", Range(0, 1)) = 1
        _ForeheadLength("Forehead Length", Range(0, 1)) = 1
        _ForeheadScale ("Forehead Scale", Range(0,1)) = 1

        _Color1("Top", Color) = (1,1,1,1)
        _Color2("Bottom", Color) = (1,1,1,1)

        _MainTex("Tex", 2D) = "white" {}
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
                float4 screenPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _ChinWidth, _ChinLength, _ForeheadWidth, _ForeheadLength;
            float _ForeheadScale, _ChinScale;
            float4 _Color1, _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z)/60, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                float value =  pow(abs(uv.x*2-1), (_ForeheadWidth*4.9 + 0.1)) + pow(abs((uv.y*(_ForeheadScale*2 + 2))-((_ForeheadScale*2 + 2)/2)), (_ForeheadLength*4.9 + 0.1));
                float value2 = pow(abs(uv.x*2-1), (_ChinWidth*4.9 + 0.1))     + pow(abs((uv.y*(_ChinScale*2 + 2))-((_ChinScale*2 + 2)/2)),         (_ChinLength*4.9 + 0.1));

                value = step(1, value) * step(0.5, i.uv.y);
                value2 = step(1, value2) * (1-step(0.5, i.uv.y));
                
                value += value2;
                value = 1 - value;
                

                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);
                float4 col = tex2D(_MainTex, texCoord);
                clip(value - 0.5);
                float4 result = value * lerp(_Color1, _Color2, uv.y);
                return result * col;
            }
            ENDCG
        }
    }
}
