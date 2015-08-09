#pragma exclude_renderers d3d11 xbox360

uniform sampler2D _MainTex;
uniform float4 _MainTex_TexelSize;
uniform float _Sigma;
uniform float _KernelSize;

struct v2f 
{ 
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

struct appdata_t
{
	float4 vertex : POSITION;
	float2 texcoord : TEXCOORD0;
};

v2f vert (appdata_t v) 
{
    v2f o; 
    o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
    o.uv = v.texcoord.xy;
    return o; 
}

float g(float x, float y, float sigma)
{
	float pi = 3.14159265f;	
    return  1.0f / (2.0f * pi * sigma * sigma) * exp(-(x * x + y * y) / (2.0f * sigma * sigma));
}

float4 frag (v2f i) : COLOR 
{ 
	float2 texCoord = i.uv;
	float4 c = tex2D(_MainTex, texCoord);
	
	float4 o = c * g(0,0,_Sigma);
	float sum = g(0,0,_Sigma);
	float x;
	float y;
	float2 uvOffset;
	
	for(x = -_KernelSize / 2; x <= _KernelSize / 2; ++x)
		for(y = -_KernelSize / 2; y <= _KernelSize / 2; ++y)
		{
			uvOffset = texCoord;
			uvOffset.x += x * _MainTex_TexelSize.x * 2;
			uvOffset.y += y * _MainTex_TexelSize.y * 2;
			o += tex2D(_MainTex, uvOffset) * g(x,y,_Sigma);
			sum += g(x,y,_Sigma);
		}
	o *= (1.0f / sum);
	return o;
} 