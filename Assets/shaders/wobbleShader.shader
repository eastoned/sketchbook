// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/wobbleShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _DistortionNoise ("Noise (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        _HorAmount ("Horizontal Amount", float) = 0.0
        _VertAmount ("Vertical Amount", float) = 0.0
        _DragPoint ("DragPoint", Vector) = (0.5, 0.5, 0, 0)
        _WobbleTexAnchor ("Anchor Point", Vector) = (0.5, 0.5, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "ForceNoShadowCasting"="True"}
		LOD 200
		Cull Off
        Blend One OneMinusSrcAlpha 
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert alpha
        
        
        
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _DistortionNoise;

		struct Input {
			float2 uv_MainTex;
		};
        
        fixed4 _WobbleTexAnchor;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
        float _HorAmount, _VertAmount;

        void vert (inout appdata_full v) {
          v.vertex.x += (distance(v.vertex.xy, float4(v.texcoord.x - _WobbleTexAnchor.x, v.texcoord.y - _WobbleTexAnchor.y, 0, 0)) * _HorAmount);
          v.vertex.y += (distance(v.vertex.xy, float4(v.texcoord.x - _WobbleTexAnchor.x, v.texcoord.y - _WobbleTexAnchor.y, 0, 0)) * _VertAmount);
        }

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
            o.Emission = c.rgb;//* _Color;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
            

            o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
