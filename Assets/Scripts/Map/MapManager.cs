using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private MapState mapState;

    private static MapManager instance;
    public static MapManager Instance { get { return instance; } }

    private MapState GetMapState()
    {
        return mapState;
    }

    private void Awake()
    {
        instance = this;
        //mapState = new MapState();
        //mapState.map = new BaseMapObjectState[10, 10];
        //for (int i = 0; i < 10; i++)
        //{
        //    for (int j = 0; j < 10; j++)
        //    {
        //        mapState.map[i, j] = new BaseMapObjectState(MapObjectType.Ground, 0);
        //    }
        //}
        //mapState.map[0, 0] = new BaseMapObjectState(MapObjectType.Wall, 1);
        //mapState.length = 10;
        //mapState.width = 10;
        ////File.Create("F:\\GameProject\\BombGame\\Assets\\Scripts\\Map\\map.json");
        //File.WriteAllText("F:\\GameProject\\BombGame\\Assets\\Scripts\\Map\\map.json",
        //    JsonConvert.SerializeObject(mapState));
    }
    private void Start()
    {
        LoadMapFromFile("F:\\GameProject\\BombGame\\Assets\\Scripts\\Map\\map.json");
        CreateMap(mapState);
    }
    internal bool CanMove(Vector2 playerPos, int height)
    {
        if (mapState.map[(int)playerPos.x, (int)playerPos.y].height == height)
            return true;
        return false;
    }

    private void LoadMapFromFile(string path)
    {
        Debug.Log(path);

        mapState = JsonConvert.DeserializeObject<MapState>(File.ReadAllText(path));

        Debug.Log("l:" + mapState.length + "w:" + mapState.width);

        CreateMap(mapState);

    }
    private void CreateMap(MapState mapState)
    {
        int l, w;
        l = mapState.length;
        w = mapState.width;
        Debug.Log("l:" + l + "w:" + w);

        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < w; j++)
            {
                var gb = mapState.map[i, j];
                GameObject obj = MyGameObjectPool.Instance.GetByMapObjectType(gb.type);
                if(obj != null)
                {
                    obj.transform.position = new Vector2(i, j);
                }
                else
                {
                    Debug.LogError("no GameObject type is " + gb.type);
                }
            }
        }
    }
}
