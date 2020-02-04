using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffsetter : MonoBehaviour
{
	public Material Material;
	public float Modifier;

	private void Update()
	{
		if (Material != null)
		{
			Material.SetVector("_Offset", new Vector4(transform.position.x * Modifier, 0,0,0));
		}
	}
}
