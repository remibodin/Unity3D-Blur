Shader "hidden/separable_gaussian_blur" 
{ 
	Properties 
	{ 
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Sigma ("Sigma", float) = 0.8
		_KernelSize ("Kernel Size", float) = 3
	}

	SubShader 
	{
		Tags { "Queue" = "Overlay" }
		Lighting Off 
		Cull Off 
		ZWrite Off 
		ZTest Always 

	    Pass
		{
			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag 
			#include "UnityCG.cginc"
			#include "SeparableGaussianBlur.cginc"
			ENDCG
		}
	}
}