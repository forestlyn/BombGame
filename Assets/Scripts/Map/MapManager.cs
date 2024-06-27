using MyInputSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapManager : MonoBehaviour
{
    private MapState mapState;
    private string mapFilePath;
    private int ObjectIdx;

    public GameObject WinGamePanel;
    private bool InMap(Vector2 arrayPos)
    {
        if (mapState == null)
            return false;
        return arrayPos.x >= 0 && arrayPos.y >= 0 &&
            arrayPos.x < mapState.length && arrayPos.y < mapState.width;
    }
    public List<BaseMapObjectState> MapObjs(Vector2 arrayPos)
    {
        if (InMap(arrayPos))
            return mapState[(int)arrayPos.x, (int)arrayPos.y];
        return null;
    }

    public MapObject GetMapObjectByObjID(int objID)
    {
        for (int i = 0; i < mapState.length; i++)
        {
            for (int j = 0; j < mapState.width; j++)
            {
                foreach (BaseMapObjectState state in mapState[i, j])
                {
                    if (state.objectId == objID)
                    {
                        return state.mapObject;
                    }
                }
            }
        }
        return null;
    }
    private static MapManager instance;
    public static MapManager Instance { get { return instance; } }

    private static int offset_x;
    private static int offset_y;
    public MapState GetMapState()
    {
        return mapState;
    }
    public void SetMapState(MapState state)
    {
        Debug.Log("set mapstate" + (state == mapState));
        mapState = state;
        RefreshMap(mapState);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        MyEventSystem.Instance.OnBoxTargetStateChange += OnObjStateChange;
        MyEventSystem.Instance.OnFlagStateChange += OnObjStateChange;
    }



    internal bool PlayerCanMove(Vector2 playerPos, Vector2 dir)
    {
        var pos = CalMapPos(playerPos + dir);
        if (!InMap(pos))
            return false;
        foreach (var obj in MapObjs(pos))
        {
            //Debug.Log(pos);
            switch (obj.type)
            {
                case MapObjectType.Box:
                    if (obj.boxMaterialType == BoxMaterialType.Wood &&
                        BoxCanMove(playerPos + dir + dir, dir))
                        continue;
                    else
                        return false;
                case MapObjectType.Ground:
                    continue;
                case MapObjectType.BoxTarget:
                    continue;
                case MapObjectType.Flag:
                    continue;
                case MapObjectType.Water:
                    continue;
                case MapObjectType.Player:
                    continue;
                case MapObjectType.Bomb:
                    if (BombCanMove(playerPos + dir + dir, dir))
                    {
                        continue;
                    }
                    else return false;
                default: return false;
            }
        }
        return true;
    }

    public bool BoxCanMove(Vector2 boxPos, Vector2 dir)
    {
        var pos = CalMapPos(boxPos);
        if (InMap(pos))
        {
            foreach (var obj in MapObjs(pos))
            {
                switch (obj.type)
                {
                    case MapObjectType.Ground:
                    case MapObjectType.BoxTarget:
                    case MapObjectType.Flag:
                    case MapObjectType.Water:
                        continue;
                    case MapObjectType.Player:
                    case MapObjectType.Wall:
                        return false;
                    case MapObjectType.Bomb:
                        return BombCanMove(boxPos + dir, dir, true);
                    case MapObjectType.Box:
                        return false;
                    default:
                        Debug.Log(obj.type);
                        return false;
                }
            }
            return true;
        }
        return false;
    }
    public bool BombCanMove(Vector2 bombPos, Vector2 dir, bool PushedByBox = false)
    {
        var pos = CalMapPos(bombPos);
        if (InMap(pos))
        {
            foreach (var obj in MapObjs(pos))
            {
                if (obj.height == 0) continue;
                else if (obj.type == MapObjectType.Box && 
                    obj.boxMaterialType != BoxMaterialType.Stone
                    && !PushedByBox && BoxCanMove(bombPos + dir, dir))
                {
                    continue;
                }
                else
                {
                    Debug.Log("bomb can't move 1" + obj.type);
                    return false;
                }
            }
            Debug.Log("bomb can move");
            return true;
        }
        Debug.Log("bomb can't move");
        return false;
    }
    public void ReStart()
    {
        LoadMapFromFile(mapFilePath);
    }
    public void LoadMapFromFile(string path)
    {
        DeleteMap();
        //Debug.Log(path);
        mapFilePath = path;
        mapState = JsonConvert.DeserializeObject<MapState>(File.ReadAllText(path));

        Debug.Log("l:" + mapState.length + "w:" + mapState.width);

        CreateMap(mapState);
    }

    private void DeleteMap()
    {
        var objs = transform.GetComponentsInChildren<Transform>();
        foreach (var obj in objs)
        {
            if (obj == transform || obj.GetComponent<MapObject>() == null)
            {
                continue;
            }
            MyGameObjectPool.Instance.Return(obj.gameObject, obj.GetComponent<MapObject>().type);
        }
    }

    private void RefreshMap(MapState mapState)
    {
        //CreateMap(mapState);
    }
    private void CreateMap(MapState mapState)
    {
        ObjectIdx = 0;
        int l, w;
        l = mapState.length;
        w = mapState.width;
        offset_x = -l / 2;
        offset_y = -w / 2;
        CheckGameWin.Clear();
        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < w; j++)
            {
                foreach (BaseMapObjectState gb in mapState[i, j])
                {
                    if (gb.type == MapObjectType.Flag || gb.type == MapObjectType.BoxTarget)
                    {
                        CheckGameWin.Add(gb);
                    }
                    GameObject obj;
                    if (gb.type == MapObjectType.Player)
                    {
                        obj = Player.Instance.gameObject;
                        Player.Instance.ResetPlayer();
                        if (obj == null) Debug.LogError("no player");
                    }
                    else if (gb.type == MapObjectType.Box)
                    {
                        obj = BoxFactory.Instance.GenerateBox(gb);
                    }
                    else
                    {
                        obj = MyGameObjectPool.Instance.GetByMapObjectType(gb.type);
                    }
                    if (gb == null) Debug.Log("11");
                    if (obj != null)
                    {
                        obj.transform.position = new Vector2(i + offset_x, j + offset_y);
                        gb.mapObject = obj.GetComponent<MapObject>();
                        if (gb.mapObject != null)
                        {
                            //gb.mapObject.id = gb.id;
                            gb.mapObject.objectId = gb.objectId = ObjectIdx;
                            gb.mapObject.open = gb.open;
                            gb.mapObject.type = gb.type;
                            //Debug.Log("type:" + gb.type + "id:" + gb.mapObject.id);
                        }
                    }
                    else
                    {
                        Debug.LogError("no GameObject type is " + gb.type);
                    }
                    ObjectIdx++;
                }
            }
        }
        for(int i = 0; i < mapState.length; i++)
        {
            for(int j = 0; j < mapState.width; j++)
            {
                foreach(BaseMapObjectState gb in mapState[i, j])
                {
                    gb.mapObject.Initialize();
                }
            }
        }
    }

    public void AddNewObj(Vector2 arrayPos, BaseMapObjectState baseMapObjectState)
    {
        baseMapObjectState.mapObject.objectId = baseMapObjectState.objectId = ObjectIdx;
        baseMapObjectState.mapObject.type = baseMapObjectState.type;
        ObjectIdx++;
        MapObjs(arrayPos).Add(baseMapObjectState);
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
    /// �����¼���map[ArrayPos]
    /// </summary>
    /// <param name="mapEvent">�¼�����</param>
    /// <param name="arrayPos">����������������λ��</param>
    /// <param name="worldPos">�¼�����λ��</param>
    /// <param name="command"></param>
    public void InvokeEvent(MapEventType mapEvent, Vector2 arrayPos, Vector2 worldPos, Command command)
    {
        List<BaseMapObjectState> list = MapObjs(arrayPos);
        if (list == null) return;
        //Debug.Log(arrayPos);

        for (int i = 0; i < list.Count; i++)
        {
            BaseMapObjectState obj = list[i];
            //if (obj == null)
            //{
            //    Debug.Log(arrayPos + " " + i + "err");
            //}
            //else
            //    Debug.Log(arrayPos + " " + i + " " + obj.type);
            obj?.mapObject?.HandleEvent(mapEvent, worldPos, command);
        }
    }
    public void InvokeEventId(MapEventType mapEvent, Vector2 arrayPos, Vector2 worldPos, Command command, int id)
    {
        List<BaseMapObjectState> list = MapObjs(arrayPos);
        for (int i = 0; i < list.Count; i++)
        {
            BaseMapObjectState obj = list[i];
            //if (obj.mapObject.id != id) continue;
            obj.mapObject?.HandleEvent(mapEvent, worldPos, command);
        }
    }
    public void InvokeEventAllId(MapEventType mapEvent, Vector2 worldPos, Command command, int id)
    {
        int l = mapState.length;
        int w = mapState.width;
        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < w; j++)
            {
                InvokeEventId(mapEvent, new Vector2(i, j), worldPos, command, id);
            }
        }
    }

    public void MoveMapObj(Vector2 pre, Vector2 after, MapObject mapObject)
    {
        var preArray = CalMapPos(pre);
        var afterArray = CalMapPos(after);
        BaseMapObjectState obj = mapState.RemoveLast(preArray, mapObject);
        mapState[afterArray].Add(obj);
        InvokeEvent(MapEventType.Leave, preArray, pre, null);
        InvokeEvent(MapEventType.Arrive, afterArray, after, null);
    }

    public BaseMapObjectState RemoveMapObj(MapObject mapObject)
    {
        var obj = mapState.RemoveLast(mapObject.ArrayPos, mapObject);
        if (obj == null)
        {
            Debug.LogWarning("remove fail!");
        }
        return obj;
    }

    public bool CheckType(Vector2 arrayPos, MapObjectType mapObjectType)
    {
        //Debug.Log("check");
        var list = MapObjs(arrayPos);
        foreach (var item in list)
        {
            //Debug.Log(item.type);
            if (item.type == mapObjectType)
                return true;
        }
        return false;
    }

    private void OnObjStateChange(bool open, int objId)
    {
        //Debug.Log("OnObjStateChange :" + open + objId);
        if (CheckGameWin.CheckWin())
        {
            GameManager.Instance.WinGame();
        }
    }

    public void WinGame()
    {
        WinGamePanel.SetActive(true);
    }

    public void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }

}
