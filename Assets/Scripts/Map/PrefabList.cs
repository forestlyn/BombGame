using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "MyAsset/PrefabList")]
public class PrefabList : ScriptableObject
{
    public MyPrefab[] prefabs;

    public GameObject GetPrefabByType(MapObjectType type)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab.type == type)
            {
                return prefab.prefab;
            }
        }
        return null;
    }

    public GameObject GetPrefabs<T>() where T : class
    {
        foreach (var prefab in prefabs)
        {
            if (prefab.GetComponent<T>() != null)
            {
                return prefab.prefab;
            }
        }
        return null;
    }
}
