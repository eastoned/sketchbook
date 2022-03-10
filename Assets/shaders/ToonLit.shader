Shader "Unlit/ToonLit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]
        _Color("Albedo Color", Color) = (0.4,0.4,0.4,1)
        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
            _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"
    "PassFlags" = "OnlyDirectional"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color, _AmbientColor;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 viewDir = normalize(i.viewDir);
                
                float3 normal = normalize(i.worldNormal);
                float4 rimDot = 1 - dot(viewDir, normal);
                
                float NdotL = dot(_WorldSpaceLightPos0, normal);

                float rimIntensity = rimDot;// *pow(NdotL, _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor;

                float lightIntensity = NdotL > 0 ? 1 : 0;
                float4 light = lightIntensity * length(_LightColor0);
                //just get hue of light color
                fixed4 col = tex2D(_MainTex, i.uv) * (_Color * _LightColor0);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col = lerp(col, _AmbientColor, 1-light);
                col = lerp(col, rim, rimIntensity);
                return col;
            }
            ENDCG
        }
    }
}
