using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldInitializer : MonoBehaviour
{
    public Button Button;
    public Text Name;
    public WorldVariable CurrentWorld;

    public void Init(World world, UnityEvent onClick)
    {
        Button.onClick.AddListener(() => CurrentWorld.CurrentValue = world);
        Button.onClick.AddListener(onClick.Invoke);
        Name.text = world.Name;
    }
}