using MyInputSystem;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapManager : MonoBehaviour
{
    private MapState mapState;

    private bool InMap(Vector2 arrayPos)
    {
        if (mapState == null)
            return false;
        return arrayPos.x >= 0 && arrayPos.y >= 0 &&
            arrayPos.x < mapState.length && arrayPos.y < mapState.width;
    }
    private BaseMapObjectState MapObj(Vector2 arrayPos)
    {
        if (InMap(arrayPos))
            return mapState.Map[(int)arrayPos.x, (int)arrayPos.y];
        return null;
    }

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
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        LoadMapFromFile("D:\\Bomb\\BombGame\\Assets\\Scripts\\Map\\map.json");
        //CreateMap(mapState);
    }
    internal bool PlayerCanMove(Vector2 playerPos,Vector2 dir, int height)
    {
        var pos = CalMapPos(playerPos + dir);
        if (!InMap(pos))
            return false;
        var obj = MapObj(pos);
        if (obj != null)
        {
            switch (obj.type)
            {
                case MapObjectType.Box:
                    return BoxCanMove(playerPos + dir + dir);
                case MapObjectType.Ground: 
                    return true;

            }
            
        }
        return false;
    }

    public bool BoxCanMove(Vector2 boxPos)
    {
        var pos = CalMapPos(boxPos);
        if (InMap(pos) && MapObj(pos).height == 0)
            return true;
        return false;
    }

    public void SwapMapObjByWorldPos(Vector2 worldPos1, Vector2 worldpos2)
    {
        //Debug.Log($"{worldPos1} {worldpos2}");
        var arrayPos1 = CalMapPos(worldPos1);
        var arrayPos2 = CalMapPos(worldpos2);
        //Debug.Log($"{arrayPos1} {arrayPos2}");
        var obj1 = MapObj(arrayPos1);
        var obj2 = MapObj(arrayPos2);
        mapState.Map[(int)arrayPos1.x, (int)arrayPos1.y] = obj2;
        mapState.Map[(int)arrayPos2.x, (int)arrayPos2.y] = obj1;
        //Debug.Log("swap!");
    }

    private void LoadMapFromFile(string path)
    {
        Debug.Log(path);

        mapState = JsonConvert.DeserializeObject<MapState>(File.ReadAllText(path));

        //Debug.Log("l:" + mapState.length + "w:" + mapState.width);

        CreateMap(mapState);

    }
    private void CreateMap(MapState mapState)
    {
        int l, w;
        l = mapState.length;
        w = mapState.width;
        offset_x = -l / 2;
        offset_y = -w / 2;
        //Debug.Log("l:" + l + "w:" + w);

        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < w; j++)
            {
                var gb = mapState.Map[i, j];
                if (gb.type != MapObjectType.Ground)
                {
                    MyGameObjectPool.Instance.GetByMapObjectType(MapObjectType.Ground).transform.position = new Vector2(i + offset_x, j + offset_y);
                }
                GameObject obj = MyGameObjectPool.Instance.GetByMapObjectType(gb.type);
                if (obj != null)
                {
                    obj.transform.position = new Vector2(i + offset_x, j + offset_y);
                    mapState.Map[i, j].mapObject = obj.GetComponent<MapObject>();
                }
                else
                {
                    Debug.LogError("no GameObject type is " + gb.type);
                }
            }
        }
    }

    public static Vector2 CalMapPos(int x, int y)
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

    public static bool InRange(Vector2 happen, Vector2 pos, int range)
    {
        switch (range)
        {
            case 4:
                return InFourRange(happen, pos);
            default:
                break;
        }
        return false;
    }
    private static bool InFourRange(Vector2 happen, Vector2 pos)
    {
        if (happen.x == pos.x && happen.y == pos.y - 1)
        {
            return true;
        }
        if (happen.x == pos.x && happen.y == pos.y + 1)
        {
            return true;
        }
        if (happen.x - 1 == pos.x && happen.y == pos.y)
        {
            return true;
        }
        if (happen.x + 1 == pos.x && happen.y == pos.y)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 发送事件给map[ArrayPos]
    /// </summary>
    /// <param name="mapEvent">事件类型</param>
    /// <param name="arrayPos">发送物体所在数组位置</param>
    /// <param name="worldPos">事件发生位置</param>
    /// <param name="command"></param>
    public void InvokeEvent(MapEventType mapEvent, Vector2 arrayPos, Vector2 worldPos, Command command)
    {
        BaseMapObjectState obj = MapObj(arrayPos);
        obj?.mapObject?.HandleEvent(mapEvent, worldPos, command);
    }
}
