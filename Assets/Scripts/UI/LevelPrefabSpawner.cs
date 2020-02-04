using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.Events;

public class LevelPrefabSpawner : UIPrefabSpawner
{
    public WorldVariable SelectedWorld;
    
    protected override void Initialize(GameObject go, object item, UnityEvent onClick)
    {
        LevelInitializer levelInitializer = go.GetComponent<LevelInitializer>();
        Level level = item as Level;
        if (level != null)
        {
            levelInitializer.Init(level, onClick);
        }
    }

    public override IEnumerator GetEnumerator()
    {
        return SelectedWorld.CurrentValue.Levels.GetEnumerator();
    }
}
