Shader "Unlit/TearQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amount("Amount", Range(0,50)) = 0.5
        _Radius ("Radius", Range(-1,1)) = 0.5
        _Mani ("Runt", Range(-1,1)) = 0.5
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
            float _Amount, _Radius, _Mani;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = float2(i.uv.x, frac(_Amount*i.uv.y+(_Time.w)));
                float value = pow(_Radius*sin(i.uv.y*3.14), 2) - pow(abs(uv.x*2-1) + uv.y, 2) - pow(abs(uv.y*2-1), 2);
                value = step(0, value);
                fixed4 col = tex2D(_MainTex, uv);
                clip(value - 0.5);
                //return i.uv.yyyy;
                return value.xxxx * i.uv.y;
            }
            ENDCG
        }
    }
}
