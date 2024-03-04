// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CustomLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]
        _Color("Albedo Color", Color) = (0.4,0.4,0.4,1)
        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

    }

    SubShader{
        Pass{

            Tags {
				"LightMode" = "ForwardBase"
			}
            CGPROGRAM

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"

            struct VertexData {
                float4 position : POSITION;
                float3 normal : NORMAL;
            };
            
            struct Interpolators {
                float4 position : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            Interpolators MyVertexProgram (VertexData v) {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.normal = normalize(i.normal);
                return i;
            }
        
            float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
                i.normal = normalize(i.normal);
                return dot(i.normal, _WorldSpaceLightPos0.xyz);
            }

            ENDCG
        }

        Pass{

            Tags {
				"LightMode" = "ForwardAdd"
			}
            CGPROGRAM

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"
            //#include "My Lighting.cginc"

            struct VertexData {
                float4 position : POSITION;
                float3 normal : NORMAL;
            };
            
            struct Interpolators {
                float4 position : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            Interpolators MyVertexProgram (VertexData v) {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.normal = normalize(i.normal);
                return i;
            }
        
            float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
                //i.normal = normalize(i.normal);
                //i.normal.y += sin(i.normal.x + _Time.z);
                return float4(0,0,0,0);
            }

            ENDCG
        }
    }
}
