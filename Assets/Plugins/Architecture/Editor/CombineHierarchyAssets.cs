using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.VersionControl;
using Object = UnityEngine.Object;

namespace Architecture
{
    public class CombineHierarchyAssets {

        [MenuItem("Assets/Combine")]
        public static void CombineSelected()
        {
            var selected = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
            string directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selected[0]));
            string parentPath = Path.Combine(directory, "New Parent.asset");
            
            EmptySO emptySoParent = ScriptableObject.CreateInstance<EmptySO>();
            AssetDatabase.CreateAsset(emptySoParent, parentPath);
            List<Object> objects = new List<Object>(); 
            for (var i = 0; i < selected.Length; i++)
            {
                objects.Add(ScriptableObject.Instantiate(selected[i]));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected[i]));
            }
            AssetDatabase.SaveAssets();

            var parent = AssetDatabase.LoadAssetAtPath<EmptySO>(parentPath);
            
            for (var i = 0; i < objects.Count; i++)
            {
                AssetDatabase.AddObjectToAsset(objects[i], parent);
            }
            
//            AssetDatabase.AddObjectToAsset(ScriptableObject.CreateInstance<EmptySO>(), parent);
            AssetDatabase.SaveAssets();
        }
    
    
        [MenuItem("Assets/Combine", true)]
        public static bool CombineSelectedValidate()
        {
            var selected = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
            var hasChildAssets = selected.Any(x=> AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(x)).Length > 1);
            return selected.Length > 1 && !selected.Any(AssetDatabase.IsSubAsset) && !hasChildAssets && !selected.Any(x=>x is DefaultAsset);
        }
    
    }
}
