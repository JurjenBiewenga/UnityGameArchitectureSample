using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class WorldPrefabSpawner : UIPrefabSpawner {
    protected override void Initialize(GameObject go, object item, UnityEvent onClick)
    {
        World world = item as World;
        if(world != null)
            go.GetComponent<WorldInitializer>().Init(world, onClick);
    }
}
