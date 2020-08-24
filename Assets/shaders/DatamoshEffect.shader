Shader "Custom/DatamoshEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MousePosition("Mouse", Vector) = (0,0,0,0)
        _MouseDirection("Dir", Vector) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        GrabPass
{
    "_PR"
}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraMotionVectorsTexture;
            sampler2D _PR;
            Vector _MousePosition, _MouseDirection;
            Vector _Colour;
            int _Button;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 mot = tex2D(_CameraMotionVectorsTexture, i.uv) * 10;
                
                #if UNITY_UV_STARTS_AT_TOP
                float2 uv = float2(i.uv.x,1-i.uv.y);
                #else
                float2 uv = float2(i.uv.x,i.uv.y);
                #endif
                
                
               // uv.y += _MouseDirection.y * step(_MousePosition.y, 1-uv.y);//lerp(1 - step(_MousePosition.y, 1-uv.y), step(_MousePosition.y,1-uv.y), _MouseDirection.z);
                uv.y += (_MouseDirection.y * lerp(1 - step(_MousePosition.y, 1-uv.y), step(_MousePosition.y, 1-uv.y), _MouseDirection.z))/_ScreenParams.y;
                
                uv.x += (_MouseDirection.x * lerp(1 - step(_MousePosition.x, uv.x), step(_MousePosition.x, uv.x), _MouseDirection.w))/_ScreenParams.x;//lerp(1 - step(_MousePosition.x, uv.x), step(_MousePosition.x, uv.x), _MouseDirection.w);
                //split into three
                //float4 colr = _Colour * lerp(1 - step(_MousePosition.y, 1-uv.y), step(_MousePosition.y, 1-uv.y), _MouseDirection.z);
                
                
                //uv.x = lerp(uv, i.uv + (_MouseDirection.x), step(_MousePosition.x, uv.x+.01));
                fixed4 pixelData = tex2D(_PR, uv);
                fixed4 col = lerp(tex2D(_MainTex, i.uv), pixelData, _Button);
                
                return col;
            }
            ENDCG
        }
    }
}
