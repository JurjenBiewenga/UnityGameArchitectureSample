using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyRuntimeSet CurrentEnemies;
    public Item[] ItemsToDrop;

    private void OnEnable()
    {
        CurrentEnemies.Add(this);
    }

    private void OnDisable()
    {
        CurrentEnemies.Remove(this);
    }
}
