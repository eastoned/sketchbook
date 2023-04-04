Shader "Unlit/EyeShader"
{
    Properties
    {
        _LPosX("Left Eye X Pos", Range(0,0.5)) = 0.5
        _LPosY("Left Eye Y Pos", Range(0.3,.75)) = 0.5
        _LSizeX("Left Eye Width", Range(0.01, 0.25)) = 0.25
        _LSizeY("Left Eye Height", Range(0.02, 0.25)) = 0.25
        
        _RPosX("Right Eye X Pos", Range(0.5,1)) = 0.75
        _RPosY("Right Eye Y Pos", Range(0.3,.75)) = 0.5
        _RSizeX("Right Eye Width", Range(0.01, 0.25)) = 0.25
        _RSizeY("Right Eye Height", Range(0.02, 0.25)) = 0.25
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float _LPosX,
                _LPosY,
                _LSizeX,
                _LSizeY,
                _RPosX,
                _RPosY,
                _RSizeX,
                _RSizeY;

            fixed4 frag(v2f i) : SV_Target
            {

                
                float l = step(.5, 1-i.uv.x) * step(_LSizeX, distance(_LPosX, i.uv.x)) + step(_LSizeY, distance(_LPosY, i.uv.y));
                l += step(.5, i.uv.x);
                

                float r = step(.5, i.uv.x) * step(_RSizeX, distance(_RPosX, i.uv.x)) + step(_RSizeY, distance(_RPosY, i.uv.y));
                r += step(.5, 1 - i.uv.x);
                float eye = l * r;

                fixed4 col = float4(eye.xxx, 0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
