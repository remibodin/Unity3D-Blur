#pragma exclude_renderers d3d11 xbox360

uniform sampler2D _MainTex;
uniform float4 _MainTex_TexelSize;
uniform float _Sigma;
uniform float _KernelSize;
uniform float4 _DirectionPass;

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

float g(float x, float sigma)
{
	float pi = 3.14159265f;
	return  1.0f / (2.0f * pi * sigma * sigma) * exp(-(x * x) / (2.0f * sigma * sigma));
}

float4 frag (v2f i) : COLOR 
{ 
	float2 texCoord = i.uv;
	float4 c = tex2D(_MainTex, texCoord);
	
	float4 o = 0;
	float sum = 0;
	float2 uvOffset;
	
	for(int kernelStep = -_KernelSize / 2; kernelStep <= _KernelSize / 2; ++kernelStep)
	{
		uvOffset = texCoord;
		uvOffset.x += kernelStep * _MainTex_TexelSize.x * _DirectionPass.x ;
		uvOffset.y += kernelStep * _MainTex_TexelSize.y * _DirectionPass.y ;
		o += tex2D(_MainTex, uvOffset) * g(kernelStep,_Sigma);
		sum += g(kernelStep,_Sigma);
	}
	o *= (1.0f / sum);
	return o;
} 