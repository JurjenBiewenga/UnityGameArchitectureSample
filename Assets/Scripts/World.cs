using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Architecture;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class World : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public LevelStaticSet Levels;

    public bool Completed
    {
        get
        {
            return Levels == null || Levels.All(x => x.Completed);
        }
    }
}
