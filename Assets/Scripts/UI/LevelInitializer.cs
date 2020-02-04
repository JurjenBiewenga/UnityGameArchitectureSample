using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour {

	public Button Button;
	public Text Name;
	public LevelVariable CurrentLevel;

	public void Init(Level level, UnityEvent onClick)
	{
		Button.onClick.AddListener(() => CurrentLevel.CurrentValue = level);
		Button.onClick.AddListener(onClick.Invoke);
		Name.text = level.Name;
	}
}
