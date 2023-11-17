using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyPrefab", menuName = "MyAsset/MyPrefab")]
public class MyPrefab : ScriptableObject
{
    public MapObjectType type;
    public GameObject prefab;
}