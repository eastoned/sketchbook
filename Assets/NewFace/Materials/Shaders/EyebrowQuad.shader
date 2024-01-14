Shader "Unlit/EyebrowQuad"
{
    Properties
    {
        _EyebrowCount ("Eyebrow Count", Range(0, 1)) = 1
        _EyebrowThickness ("Eyebrow Thickness", Range(0, 1)) = 0
        _EyebrowRoundness ("Eyebrow Roundness", Range(0, 1)) = 1
        _Rounded ("Rounded", Range(1, 256)) = 40

        _EyebrowCurve ("Eyebrow Curve", Range(0, 1)) = 0

        _Color1 ("Inner", Color) = (1,1,1,1)
        _Color2 ("Outer", Color) = (1,1,1,1)

        _PositionMomentum ("Position Momementum", Vector) = (0,0,0)

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

            float _EyebrowCount;
            float _EyebrowThickness;
            float _EyebrowRoundness;
            float _EyebrowCurve;
            float4 _Color1, _Color2;
            float _Rounded;

            float4 _PositionMomentum;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z-1.5)/20, v.vertex.z, v.vertex.w);

                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.z+2)/10, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float pullStrength = smoothstep(0, 1, distance(float2(0.5,0.5), i.uv));
                float2 uv = float2(i.uv.x + (_PositionMomentum.x * pullStrength), i.uv.y + (_PositionMomentum.y * pullStrength));
                float stretchUV = sin(uv.x*3.14);
                uv.y += .01*sin(uv.x*8-_Time.z*3);
                uv *= float2((int)(_EyebrowCount*15)+1, 1);
                uv = frac(uv);
                uv += float2(0, stretchUV * (_EyebrowCurve*2 -1) * ((_EyebrowThickness*7 + 1)-1)/16);

                float result = pow(abs(uv.x*2-1), (_EyebrowRoundness*3.5+0.5)) + pow(abs(uv.y*2-1)*(_EyebrowThickness*7+1), (_EyebrowRoundness*3.5+0.5));
                result = 1-step(1, result);
                clip(result-0.5);
                float4 eyebrowCol = result * lerp(_Color1, _Color2, i.uv.x);

                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);
                float4 col = tex2D(_MainTex, texCoord);
                //float result = sin(uv.x * _EyebrowCount * 3.14 + _EyebrowThickness)/6 - uv.y + .5;
                //float result2 = -sin(uv.x * _EyebrowCount * 3.14 + _EyebrowThickness)/6 - uv.y + 0.5;
                //float result2 = sin(
                //float final = step(0, max(result2, result)*1-step(0, min(result2, result)));
                //clip(final - 0.5);
                //final *= 0;
                
                return eyebrowCol * col;
            }
            ENDCG
        }
    }
}
