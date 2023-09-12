Shader "Unlit/EarQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale1("Scale1", Range(0, 1)) = 0
        _Scale2("Scale2", Range(0, 1)) = 0
        _Scale3("Scale3", Range(0, 1)) = 0
        _Scale4("Scale4", Range(0, 1)) = 0
        _InfluenceX ("Affector", Range(0,1)) = 1
        _InfluenceY ("Affector y", Range(0,1)) = 1
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale1, _Scale2, _Scale3, _Scale4;
            float _InfluenceX, _InfluenceY;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z/5)/20, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 fourCircle = i.uv*2;
                float circle1 = clamp(1-distance(float2(0.5,0.5),fourCircle), 0,1)* _Scale1;
                float circle2 = clamp(1-distance(float2(0.5,0.5),(fourCircle-float2(1, 0))), 0,1)* _Scale2;
                float circle3 = clamp(1-distance(float2(0.5,0.5),(fourCircle-float2(0, 1))), 0,1)* _Scale3;
                float circle4 = clamp(1-distance(float2(0.5,0.5),(fourCircle-float2(1, 1))), 0,1)* _Scale4;
                float res = (circle1 * circle2 * circle3 * circle4); 
                //dis1 = step(.25, dis1);

                float value = distance(i.uv, float2(0.5, 0.5));
                //clip(1-value - (0.5));
                float ineer = step(0.35,distance(i.uv, float2(0.5, 0.5)));
                ineer += 1-step(0.25, distance(i.uv,float2(0.25,0.25)));
                ineer = saturate(ineer);
                ineer = lerp(float4(1, 0, 0, 1)*ineer, float4(1,1,1,1)*ineer, 1-uv.y);

                return res.xxxx;
            }
            ENDCG
        }
    }
}
