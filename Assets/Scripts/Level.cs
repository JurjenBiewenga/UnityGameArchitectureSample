using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public ItemStaticSet Items;
    public EnemyStaticSet Enemies;
    public Enemy Boss;
    public int EnemyCount;
    public int MaxEnemiesOnScreen;
    public float SpawnDelay;
    public bool Completed;
}
