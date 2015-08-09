using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GaussianBlur : MonoBehaviour 
{
	public enum Algo
	{
		naive = 0,
		separable = 1
	}

	public Algo algo;
	public float sigma = 1.5f;
	public float kernelSize = 7f;

	[SerializeField]
	private Shader m_Shader;

	private Material m_Material;

	private void OnValidate()
	{
		switch (algo) 
		{
			case Algo.naive: m_Shader = Shader.Find ("hidden/naive_gaussian_blur");break;
			case Algo.separable: m_Shader = Shader.Find ("hidden/separable_gaussian_blur");break;
		}
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

		if (algo == Algo.separable) 
		{
			RenderTexture tmpRt = RenderTexture.GetTemporary(input.width, input.height);
			m_Material.SetVector ("_DirectionPass", new Vector4 (1, 0, 0, 0));
			Graphics.Blit (input, tmpRt, m_Material);
			m_Material.SetVector ("_DirectionPass", new Vector4 (0, 1, 0, 0));
			Graphics.Blit (tmpRt, output, m_Material);
			RenderTexture.ReleaseTemporary(tmpRt);
		} 
		else
		{
			Graphics.Blit (input, output, m_Material);
		}
	}
}
