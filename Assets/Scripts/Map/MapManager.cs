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

    private static int offset_x;
    private static int offset_y;
    private MapState GetMapState()
    {
        return mapState;
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LoadMapFromFile("F:\\GameProject\\BombGame\\Assets\\Scripts\\Map\\map.json");
        CreateMap(mapState);
    }
    internal bool CanMove(Vector2 playerPos, int height)
    {
        var pos = CalMapPos(playerPos);
        if (mapState.map[(int)pos.x, (int)pos.y].height == height)
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
        offset_x = -l / 2;
        offset_y = -w / 2;
        Debug.Log("l:" + l + "w:" + w);

        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < w; j++)
            {
                var gb = mapState.map[i, j];
                GameObject obj = MyGameObjectPool.Instance.GetByMapObjectType(gb.type);
                if(obj != null)
                {
                    obj.transform.position = new Vector2(i + offset_x, j + offset_y);
                }
                else
                {
                    Debug.LogError("no GameObject type is " + gb.type);
                }
            }
        }
    }

    public static Vector2 CalMapPos(int x,int y)
    {
        return new Vector2(x - offset_x, y - offset_y);
    }
    public static Vector2 CalMapPos(Vector2 pos)
    {
        return new Vector2(pos.x - offset_x, pos.y - offset_y);
    }
    public static Vector2 CalWorldPos(int x, int y)
    {
        return new Vector2(x + offset_x, y + offset_y);
    }
    public static Vector2 CalWorldPos(Vector2 pos)
    {
        return new Vector2(pos.x + offset_x, pos.y + offset_y);
    }
}
