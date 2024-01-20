Shader "Unlit/MouthQuadBean"
{
    Properties
    {

        _MouthRadius ("Mouth Radius", Range(0, 1)) = 0.5

        _MouthOpen ("Mouth Open", Range(0, 1)) = 0.5
        _MouthBend ("Mouth Bend", Range(0, 1)) = 0.5

        _TeethTop ("Top Teeth", Range(0,1)) = 0.5
        _TeethBottom ("Bottom Teeth", Range(0,1)) = 0.5

        _TeethCount("Teeth Count", Range(0, 1)) = .5
        _TeethRoundness("Teeth Roundness", Range(0, 1)) = 1

        _TongueRadius("Tongue Radius", Range(0, 1)) = 0.5
        _TongueScale("Tongue Scale", Range(0, 1)) = 0.5
        _TongueHeight("Tongue Height", Range(0, 1)) = 0

        _Color1("Mouth Bottom", Color) = (1,1,1,1)
        _Color2("Mouth Top", Color) = (1,1,1,1)
        _Color3("Tongue Bottom", Color) = (1,1,1,1)
        _Color4("Tongue Top", Color) = (1,1,1,1)

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

            float _MouthRadius, _MouthOpen, _MouthBend;
            float _TeethTop, _TeethBottom;
            float _TongueRadius, _TongueScale, _TongueHeight;
            float4 _Color1, _Color2, _Color3, _Color4;
            float _TeethCount;
            float _TeethRoundness;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = float4(v.vertex.x, v.vertex.y + sin(_Time.w-2)/60, v.vertex.z, v.vertex.w);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv += float2(0, (_MouthBend*2-1)*pow((uv.x-0.5), 2));
                uv.y += .01*sin(abs(uv.x-.5)*16-_Time.w*3);
                float line0 = (pow(_MouthRadius/2, 2) - pow((uv.x-0.5), 2)) - pow((uv.y-0.5)/_MouthOpen,2);
                fixed4 col = fixed4(0,0,0,0);
                float line1 = pow((uv.x-0.5), 2);
                line0 = step(0, line0);
                clip(line0 - 0.5);
                float2 teethUV = float2(frac(uv.x*(int)(_TeethCount*29+1)), (uv.y-0.5)*2);

                float topComp = teethUV.y - (1 - _TeethTop) - 0.5;
                float line4 = step(0, topComp);

                float botComp = -teethUV.y - (1-_TeethBottom) - 0.5;
                line4 += step(0, botComp);

                float toof = pow(abs(teethUV.x*2-1), (3.5*_TeethRoundness+.5)) + pow(abs(topComp*2), (3.5*_TeethRoundness+.5));
                float toof2 = pow(abs(teethUV.x*2-1), (3.5*_TeethRoundness+.5)) + pow(abs(botComp*2), (3.5*_TeethRoundness+.5));
                toof = 1-step(1, toof);
                toof2 = 1 - step(1, toof2);
                toof += toof2;
                line4 += toof;

                float tongue = pow((.125*_TongueRadius), 2) - pow((uv.x-.5) *(1-(.5*_TongueScale+.25)), 2) - pow((uv.y-(_TongueHeight))*(.5*_TongueScale+.25),2);
                tongue = step(0, tongue);

                float4 tonColor = tongue * lerp(_Color3, _Color4, uv.y);//lerp(_Color3, _Color4, ((uv.y-_TongueHeight)*_TongueScale));
                float4 mouthColor = (1-tongue) * lerp(_Color1, _Color2, i.uv.y);

                float2 texCoord = i.screenPosition.xy/i.screenPosition.w;
                float aspect = _ScreenParams.x/_ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);
                col = tex2D(_MainTex, texCoord) * saturate(saturate(tonColor + mouthColor) + line4);

                return col;
            }
            ENDCG
        }
    }
}
