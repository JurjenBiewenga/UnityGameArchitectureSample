using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public FloatReference Rarity;
    public Sprite Icon;
}
