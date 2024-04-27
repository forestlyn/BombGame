using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapState
{
    public int length, width;
    public List<BaseMapObjectState>[,] Map;
    public List<BaseMapObjectState> this[int x, int y]
    {
        get
        {
            return Map[x, y];
        }
    }
    public List<BaseMapObjectState> this[Vector2 v]
    {
        get
        {
            return Map[(int)v.x, (int)v.y];
        }
    }
    public BaseMapObjectState RemoveLast(int x, int y, MapObject mapObject)
    {
        if (Map[x, y].Count == 0) return null;
        var obj = Map[x, y].Find(obj => obj.objectId == mapObject.objectId);
        Map[x, y].Remove(obj);
        return obj;
    }
    public BaseMapObjectState RemoveLast(Vector2 v, MapObject mapObject)
    {
        return RemoveLast((int)v.x, (int)v.y, mapObject);
    }
}

public class BaseMapObjectState
{
    public MapObjectType type;
    public int height;
    /// <summary>
    /// 配对id id相同代表一对 如压力板对应的门
    /// </summary>
    public int id;
    /// <summary>
    /// global map obj unique id
    /// </summary>
    [JsonIgnore]
    public int objectId;
    public bool open;

    //box
    public BoxMaterialType boxMaterialType;
    public KEDeliverType boxkEType;
    public int boxDir;
    public int boxRotateAngle;
    public int boxAdd;
    public int boxMulti;


    [JsonIgnore]
    public MapObject mapObject;
    public BaseMapObjectState(MapObjectType type, int height)
    {
        this.type = type;
        this.height = height;
    }
}