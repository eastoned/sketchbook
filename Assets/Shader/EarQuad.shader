Shader "Unlit/EarQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale1("Scale1", Range(0, 1)) = 0
        _Scale2("Scale2", Range(-1, 1)) = 0
        _Scale4("Scale4", Range(1, 6)) = 0
        _Scale6("Scale6", Range(0.6, 1.25)) = 0


        _Scale7("Scale7", Range(0.1, 1.25)) = 0

        _Scale8("Scale8", Range(1, 1.5)) = 0
        _Scale9("Scale9", Range(1, 1.5)) = 0
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

      
            float _Scale1, _Scale2, _Scale4, _Scale6, _Scale7, _Scale8, _Scale9;


            v2f vert (appdata v)
            {
                v2f o;
                //v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z/5)/20, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv*2 + float2(-1, -1);
                float2 fourCircle = i.uv*2;

                float2 warp = uv;
                float res = pow(uv.x*_Scale1, 3*_Scale6) - pow(uv.y*_Scale2, 3) - pow(pow(uv.x*_Scale8, 2) + pow(uv.y*_Scale9, 2), 2*_Scale4);
                ///float res = pow(uv.x*_Scale4, 3*_Scale6) - pow(abs(uv.y*_Scale5), 3*_Scale7) - pow(pow(uv.x, 2) + pow(uv.y, 2), 2);
                res = step(0, res);
                return res.xxxx;
            }
            ENDCG
        }
    }
}
