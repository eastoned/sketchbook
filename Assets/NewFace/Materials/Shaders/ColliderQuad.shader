Shader "Unlit/ColliderQuad"
{
    Properties
    {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex + float4(0,0,-0.1,0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float value =  pow(abs(i.uv.x*2-1), 25) + pow(abs(i.uv.y*2-1), 25);
                clip(value - .1);

                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);
                float4 col = tex2D(_MainTex, texCoord);

                return 1 * col;
            }
            ENDCG
        }
    }
}
