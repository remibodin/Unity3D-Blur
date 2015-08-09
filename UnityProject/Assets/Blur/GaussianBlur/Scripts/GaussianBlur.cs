using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GaussianBlur : MonoBehaviour 
{
	public float sigma = 1.5f;
	public float kernelSize = 7f;

	[SerializeField]
	private Shader m_Shader;

	private Material m_Material;

	private void OnEnable()
	{
		m_Shader = Shader.Find ("hidden/naive_gaussian_blur");
		if (m_Shader.isSupported == false)
		{
			enabled = false;
			return;
		}
		m_Material = new Material (m_Shader);
	}

	private void OnRenderImage(RenderTexture input, RenderTexture output)
	{
		m_Material.SetFloat ("_Sigma", sigma);
		m_Material.SetFloat ("_KernelSize", kernelSize);
		Graphics.Blit (input, output, m_Material);
	}
}
