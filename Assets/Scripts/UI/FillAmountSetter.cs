using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FillAmountSetter : MonoBehaviour
{
	public FloatVariable Value;
	public FloatVariable MaxValue;
	public Image Target;

	public float Speed;

	private void Awake()
	{
		if (Value == null || MaxValue == null || Target == null)
			return;

		float targetPercentage = Value.CurrentValue / MaxValue.CurrentValue;
		Target.fillAmount = targetPercentage;
	}

	private void Update()
	{
		if (Value == null || MaxValue == null || Target == null)
			return;

		float targetPercentage = Value.CurrentValue / MaxValue.CurrentValue;
		Target.fillAmount = Mathf.MoveTowards(Target.fillAmount, targetPercentage, Mathf.Clamp(Mathf.Abs(targetPercentage - Target.fillAmount) * Time.deltaTime * Speed, .01f, 10));
	}
}
