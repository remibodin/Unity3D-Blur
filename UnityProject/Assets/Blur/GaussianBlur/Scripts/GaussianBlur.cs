using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GaussianBlur : MonoBehaviour 
{
	public enum Algo
	{
		NAIVE,
		TWO_PASS,
		TWO_PASS_LINEAR_SAMPLING
	}

	public enum Quality
	{
		LITTLE_KERNEL,
		MEDIUM_KERNEL,
		BIG_KERNEL
	};

	public Algo algo;
	public Quality quality;
	public float sigma = 10f;
	
	private Shader m_Shader;
	private Material m_Material;

	private void OnValidate()
	{
		Init ();
	}

	private void OnEnable()
	{
		Init ();
	}

	private void Init()
	{
		switch (algo) 
		{
			case Algo.NAIVE: m_Shader = Shader.Find ("hidden/naive_gaussian_blur");break;
			case Algo.TWO_PASS: m_Shader = Shader.Find ("hidden/two_pass_gaussian_blur");break;
			case Algo.TWO_PASS_LINEAR_SAMPLING: m_Shader = Shader.Find ("hidden/two_pass_linear_sampling_gaussian_blur");break;
		}
		if (m_Shader.isSupported == false)
		{
			enabled = false;
			Debug.LogWarning ("Shader not supported");
			return;
		}
		if (algo == Algo.NAIVE && quality == Quality.BIG_KERNEL) 
		{
			quality = Quality.MEDIUM_KERNEL;
			Debug.LogWarning("Some graphic's driver crash with Algo.NAIVE and Quality.BIG_KERNEL !");
		}
		m_Material = new Material (m_Shader);
		m_Material.EnableKeyword (quality.ToString ());
	}

	private void OnRenderImage(RenderTexture input, RenderTexture output)
	{
		m_Material.SetFloat ("_Sigma", sigma);
		Graphics.Blit (input, output, m_Material);
	}

	private bool m_displayGUI = true;

	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.D))
			m_displayGUI = !m_displayGUI;
	}

	private void OnGUI()
	{
		if (!m_displayGUI)
			return;
		GUILayout.BeginVertical ("Box");

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Algo : " + algo.ToString () + "\nKernelSize : " + quality.ToString ());
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("NAIVE")) algo = Algo.NAIVE;
		if (GUILayout.Button ("TWO_PASS")) algo = Algo.TWO_PASS;
		if (GUILayout.Button ("TWO_PASS_LINEAR_SAMPLING")) algo = Algo.TWO_PASS_LINEAR_SAMPLING;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("LITTLE_KERNEL")) quality = Quality.LITTLE_KERNEL;
		if (GUILayout.Button ("MEDIUM_KERNEL")) quality = Quality.MEDIUM_KERNEL;
		if (GUILayout.Button ("BIG_KERNEL")) quality = Quality.BIG_KERNEL;
		GUILayout.EndHorizontal ();

		GUILayout.EndVertical ();

		if (GUI.changed)
			Init ();
	}
}
