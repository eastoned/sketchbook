Shader "Unlit/HandQuad"
{
    Properties
    {

        _PalmWidth ("Palm Width", Range(0, 1)) = 0.5

        _Finger1 ("Finger 1 Length", Range(0, 1)) = 0.5
        _Finger2 ("Finger 2 Length", Range(0, 1)) = 0.5
        _Finger3 ("Finger 3 Length", Range(0, 1)) = 0.5
        _Finger4 ("Finger 4 Length", Range(0, 1)) = 0.5

        _Color1("Mouth Bottom", Color) = (1,1,1,1)
        _Color2("Mouth Top", Color) = (1,1,1,1)

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
            float _PalmWidth;
            float _Finger1, _Finger2, _Finger3, _Finger4;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-2)/60, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 uv2 = i.uv;
                float value = i.uv;

                float value2 = pow(abs(uv.x*2-1), (_PalmWidth*4.9 + 0.1)) + pow(abs((uv.y*2)-1), 5);
                value2 = step(1, value2) * 1-step(0.5, i.uv.y);
 
                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);

                float4 col = tex2D(_MainTex, texCoord);

                return value.xxxx * col;
            }
            ENDCG
        }
    }
}
