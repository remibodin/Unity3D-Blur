#define PI 3.14159265

#ifdef MEDIUM_KERNEL
	#define KERNEL_SIZE 35
#elif BIG_KERNEL
	#define KERNEL_SIZE 127
#else //LITTLE_KERNEL
	#define KERNEL_SIZE 7
#endif

float gauss(float x, float sigma)
{
	return  1.0f / (2.0f * PI * sigma * sigma) * exp(-(x * x) / (2.0f * sigma * sigma));
}

float gauss(float x, float y, float sigma)
{
    return  1.0f / (2.0f * PI * sigma * sigma) * exp(-(x * x + y * y) / (2.0f * sigma * sigma));
}