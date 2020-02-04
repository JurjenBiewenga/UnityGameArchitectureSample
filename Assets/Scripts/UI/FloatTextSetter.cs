using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FloatTextSetter : MonoBehaviour
{
	public FloatVariable Value;
	public Text Text;

	private void Update()
	{
		if (Value == null || Text == null)
			return;
		
		Text.text = Value.ToString();
	}
}
