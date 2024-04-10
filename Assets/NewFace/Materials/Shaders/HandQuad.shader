Shader "Unlit/HandQuad"
{
    Properties
    {

        _PalmWidth ("Palm Width", Range(0, 1)) = 0.5
        _WristBase ("Wrist", Range(0, 1)) = 0.5

        _Finger1 ("Finger 1 Length", Range(0, 1)) = 0.5
        _Finger2 ("Finger 2 Length", Range(0, 1)) = 0.5
        _Finger3 ("Finger 3 Length", Range(0, 1)) = 0.5
        _Finger4 ("Finger 4 Length", Range(0, 1)) = 0.5

        _Color1("Hand Bottom", Color) = (1,1,1,1)
        _Color2("Hand Top", Color) = (1,1,1,1)

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
            float _PalmWidth, _WristBase;
            float _Finger1, _Finger2, _Finger3, _Finger4;
            float4 _Color1, _Color2;

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

                float knuckles = 1 - step(1, pow(abs(frac(uv.x*4)*2 - 1), 2) + pow(abs(uv.y*8 - 4), 2));

                float fingerValue = (i.uv.y*8 - 1 - _Finger1 * 6);
                fingerValue = lerp( step(0, fingerValue) * step(.5, 1-i.uv.y), 1-step(0, fingerValue) * 1-step(.5, 1-i.uv.y), step(0.5, _Finger1)) * step(.75, 1 - i.uv.x);

                float fingerCircle = pow(abs(uv.x*8 - 1), 2) + pow(abs(uv.y*8 - 1 - _Finger1*6), 2);
                fingerCircle = 1-step(1, fingerCircle);
                //fingerValue += fingerCircle;
                fingerValue = saturate(fingerValue);


                float fingerValue2 = (i.uv.y*8 - 1 - _Finger2 * 6);
                fingerValue2 = lerp(step(0, fingerValue2) * step(.5, 1-i.uv.y), 1-step(0, fingerValue2) * 1-step(.5, 1-i.uv.y), step(0.5, _Finger2)) * step(.25, i.uv.x) * step(.5, 1 - i.uv.x);

                float fingerCircle2 = pow(abs(uv.x*8 - 3), 2) + pow(abs(uv.y*8 - 1 - _Finger2*6), 2);
                fingerCircle2 = 1-step(1, fingerCircle2);
                //fingerValue2 += fingerCircle2;
                //float fingerValue2 = (-i.uv.y + _Finger2/2 + 1.5) * step(.25, i.uv.x) * step(.5, 1 - i.uv.x);
                //fingerValue2 = step(1, fingerValue2) * step(0.5, i.uv.y);


                float fingerValue3 = (i.uv.y*8 - 1 - _Finger3 * 6);
                fingerValue3 = lerp(step(0, fingerValue3) * step(.5, 1-i.uv.y), 1-step(0, fingerValue3) * 1-step(.5, 1-i.uv.y), step(0.5, _Finger3)) * step(.5, i.uv.x) * step(.25, 1 - i.uv.x);

                float fingerCircle3 = pow(abs(uv.x*8 - 5), 2) + pow(abs(uv.y*8 - 1 - _Finger3*6), 2);
                fingerCircle3 = 1-step(1, fingerCircle3);
                //fingerValue3 += fingerCircle3;
                fingerValue3 = saturate(fingerValue3);
                //float fingerValue3 = (-i.uv.y + _Finger3/2 + 1.5) * step(.5, i.uv.x) * step(.25, 1 - i.uv.x);
                //fingerValue3 = step(1, fingerValue3) * step(0.5, i.uv.y);

                float fingerValue4 = (i.uv.y*8 - 1 - _Finger4 * 6);
                fingerValue4 = lerp(step(0, fingerValue4) * step(.5, 1-i.uv.y), 1-step(0, fingerValue4) * 1-step(.5, 1-i.uv.y), step(0.5, _Finger4)) * step(.75, i.uv.x);

                float fingerCircle4 = pow(abs(uv.x*8 - 7), 2) + pow(abs(uv.y*8 - 1 - _Finger4*6), 2);
                fingerCircle4 = 1-step(1, fingerCircle4);
                //fingerValue4 += fingerCircle4;
                //float fingerValue4 = (-i.uv.y + _Finger4/2 + 1.5) * step(.75, i.uv.x);
                //fingerValue4 = step(1, fingerValue4) * step(0.5, i.uv.y);

                float value2 = pow(abs(uv.x*2-1), (_PalmWidth*4.9 + 0.1)) + pow(abs((uv.y*2)-1), 5);
                value2 = (1-step(1, value2)) * step(0.5, 1-i.uv.y);

                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);

                
                float fingers = fingerValue + fingerValue2 + fingerValue3 + fingerValue4 + value2 + knuckles;
                fingers = saturate(fingers);
                float circles = fingerCircle + fingerCircle2 + fingerCircle3 + fingerCircle4;
                clip(fingers + circles - 0.5);
                

                float4 col = tex2D(_MainTex, texCoord) * lerp(_Color1, _Color2, saturate(i.uv.y + circles));


                return col;
            }
            ENDCG
        }
    }
}
