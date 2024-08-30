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
float4 _Color1, _Color2;
float4 _PositionMomentum;
float _Dashed;

void DashedObject(float2 texCoord)
{
    float dash = frac((texCoord.x + _Time.x + texCoord.y) * 30);
    clip(dash - _Dashed);
}