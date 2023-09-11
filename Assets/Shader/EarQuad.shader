Shader "Unlit/EarQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale("Scale", Range(0, 0.5)) = 0
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
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z/5)/10, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;


                float value = distance(i.uv, float2(0.5, 0.5));
                clip(1-value - (0.5));
                float ineer = step(0.35,distance(i.uv, float2(0.5, 0.5)));
                ineer += 1-step(0.25, distance(i.uv,float2(0.25,0.25)));
                ineer = saturate(ineer);
                return lerp(float4(1, 0, 0, 1)*ineer, float4(1,1,1,1)*ineer, 1-uv.y);
            }
            ENDCG
        }
    }
}
