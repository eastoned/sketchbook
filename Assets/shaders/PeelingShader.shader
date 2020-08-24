// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/PeelingShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        _HorAmount ("Horizontal Amount", float) = 0.0
        _VertAmount ("Vertical Amount", float) = 0.0
        _DepthAmount ("Depth Amount", float) = 0.0
        _AnchorPoint ("AnchorPoint", Vector) = (0.5, 0.5, 0, 0)
        
        _PerlinAmount ("Perlin Multiplier", float) = 0
        _PerlinNoise ("Perlin Noise Tex", 2D) = "white"{}
        
        _Transparency ("Transparency", float) = 0
        
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200
        
		CGPROGRAM
        
        
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert alpha:fade addshadow
        
        
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _PerlinNoise;
        

		struct Input {
			float2 uv_MainTex;
             float4 screenPos;
		};
        
        float4 _AnchorPoint;
	    half _Glossiness;
	    half _Metallic;
	    fixed4 _Color;
        float _HorAmount, _VertAmount, _DepthAmount, _PerlinAmount, _Transparency;
       
        void vert (inout appdata_full v) {
        
          float3 worldPos = mul (unity_ObjectToWorld, v.vertex);
          float4 tex = tex2Dlod (_PerlinNoise, float4(v.texcoord.xy * worldPos + _Time.x, 0, 0));
          
          v.vertex.x += _HorAmount * pow(distance(v.vertex.xy, _AnchorPoint), 3);

          
          v.vertex.y += _VertAmount * pow(distance(v.vertex.xy, _AnchorPoint), 3);
 
          v.vertex.z += _DepthAmount * (distance(v.vertex.xy, _AnchorPoint));
          
          v.texcoord.x += _PerlinAmount * tex.r;
          
        }

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
		    fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            
            float2 textureCoordinate = IN.screenPos.xy / IN.screenPos.w;
            float aspect = _ScreenParams.x / _ScreenParams.y;
            textureCoordinate.x = textureCoordinate.x * aspect;
            //textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex_ST);
            
            //fixed4 fade = tex2D (_PerlinNoise, textureCoordinate);
		    o.Albedo = c.rgb;
            o.Alpha = c.a * _Transparency;
            
             
             o.Emission = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
