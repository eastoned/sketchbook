Shader "Custom/PullSprite"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Vector2 ("Vector", Vector) = (0,0,0,0)
        
        _xAmount("X Amount", float) = 0
        _yAmount("Y Amount", float) = 0
        
        
        _xThresh ("X Threshold", float) = 0
        _yThresh ("Y Threshold", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Fade" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _xThresh, _yThresh, _xAmount, _yAmount;
        float4 _Vector2;
        
        void vert (inout appdata_full v) {
          v.vertex.z += _yAmount * step(v.texcoord.y, _yThresh);
          v.vertex.x += _xAmount * step(v.texcoord.x, _xThresh);
        }
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            
            float2 uv = IN.uv_MainTex;// * (distance(float2(_Vector2.x, _Vector2.y), IN.uv_MainTex) + 0.5);
            
            fixed4 c = tex2D (_MainTex, uv) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            
           // o.Normal = UnpackNormal(tex2D(norm, uv));
            o.Alpha = c.a - 0.3;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
