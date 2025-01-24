using MyInputSystem;
using MyTools.MyCoroutines;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private MapState mapState;
    private string mapFilePath;
    private string mapName;

    private int ObjectIdx;

    public GameObject WinGamePanel;
    public GameObject TipPanel;
    public GameObject InputPanel;
    public Text mapFileNameText;
    public Material gridMat;
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
        {
            //Debug.Log(arrayPos + "(" + (int)arrayPos.x + "," + (int)arrayPos.y + ")");
            //Debug.Log(arrayPos.x);
            //Debug.Log(arrayPos.y);
            //foreach (var obj in mapState[(int)arrayPos.x, (int)arrayPos.y])
            //{
            //    Debug.Log(arrayPos + " " + obj.objectId + obj.type + " " + obj.mapObject.ArrayPos + obj.mapObject.WorldPos);
            //}
            return mapState[(int)arrayPos.x, (int)arrayPos.y];
        }
        //Debug.LogWarning(arrayPos);
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
        //MyEventSystem.Instance.OnBoxTargetStateChange += OnObjStateChange;
        //MyEventSystem.Instance.OnFlagStateChange += OnObjStateChange;
        Init();
    }

    void Init()
    {
        float offset = gridMat.GetFloat("_IsShow");
        GameManager.Instance.GridOn = offset == 1 ? true : false;
    }

    public void ShowTip(bool show)
    {
        TipPanel.SetActive(show);
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
                case MapObjectType.Ground:
                case MapObjectType.BoxTarget:
                case MapObjectType.Flag:
                case MapObjectType.Water:
                case MapObjectType.Player:
                    continue;
                case MapObjectType.Box:
                    if (obj.boxMaterialType == BoxMaterialType.Wood &&
                        BoxCanMove(playerPos + dir + dir, dir))
                        continue;
                    else
                        return false;
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
        //Debug.Log("Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
        var pos = CalMapPos(boxPos);
        //Debug.Log(pos);
        if (InMap(pos))
        {
            foreach (var obj in MapObjs(pos))
            {
                //Debug.Log(obj.type);
                switch (obj.type)
                {
                    case MapObjectType.Ground:
                    case MapObjectType.BoxTarget:
                    case MapObjectType.Flag:
                    case MapObjectType.Water:
                        continue;
                    case MapObjectType.Player:
                    case MapObjectType.Wall:
                    case MapObjectType.Box:
                        //Debug.Log(obj.objectId);
                        //Debug.Log(obj.mapObject.ArrayPos);
                        return false;
                    case MapObjectType.Bomb:
                        return false;
                    default:
                        Debug.LogError(obj.type);
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
                else if (obj.type == MapObjectType.Box)
                {
                    return false;
                }
                else
                {
                    //Debug.Log("bomb can't move 1" + obj.type);
                    return false;
                }
            }
            //Debug.Log("bomb can move");
            return true;
        }
        //Debug.Log("bomb can't move");
        return false;
    }
    public void ReStart()
    {
        LoadMapFromFile(mapFilePath, mapName);
    }
    public void LoadMapFromFile(string path, string name)
    {
        DeleteMap();
        Debug.Log(path);
        mapFilePath = path;
        mapName = name;
#if UNITY_ANDROID && !UNITY_EDITOR
    StartCoroutine(LoadMapAndContinue(path, name));
#else
        mapState = JsonConvert.DeserializeObject<MapState>(File.ReadAllText(path));
        mapFileNameText.text = name;
        WaitUntil wait = new WaitUntil(() => mapState != null);
        CreateMap(mapState);
#endif
    }

    private IEnumerator LoadMapAndContinue(string path, string name)
    {
        yield return StartCoroutine(LoadMapState(path));
        mapFileNameText.text = name;
        WaitUntil wait = new WaitUntil(() => mapState != null);
        CreateMap(mapState);
    }

    private IEnumerator LoadMapState(string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            mapState = JsonConvert.DeserializeObject<MapState>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("LoadMapState err");
            Debug.LogError(path);
            Debug.LogError(request.error);
        }
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
        mapState = null;
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
        for (int i = 0; i < mapState.length; i++)
        {
            for (int j = 0; j < mapState.width; j++)
            {
                foreach (BaseMapObjectState gb in mapState[i, j])
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
        //发现一个情况下的bug,在玩家连续推箱子情况下，箱子慢一步导致arraypos还在上一步不正常
        //但是箱子逻辑上已经到了对应位置，导致mapobjs错误
        //现将arraypos四舍五入一下，但实际上感觉应当按照逻辑位置来，暂且这样解决，还是移动有过程导致的啊~
        //return new Vector2(pos.x - offset_x, pos.y - offset_y);
        return new Vector2((float)Math.Round(pos.x - offset_x), (float)Math.Round(pos.y - offset_y));
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
            Debug.LogWarning("remove fail!:" + mapObject.type + " " + mapObject.objectId);
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

    //private void OnObjStateChange(bool open, int objId)
    //{
    //    //Debug.Log("OnObjStateChange :" + open + objId);
    //    //MyCoroutines.StartCoroutine(CheckGameState(0.0f));
    //    CheckGameState();
    //}
    //public IEnumerator CheckGameState(float time)
    //{
    //    yield return new YieldWaitForSeconds(0.4f);
    //    CheckGameState();
    //}

    public void CheckGameState()
    {
        if (CheckGameWin.CheckWin())
        {
            GameManager.Instance.WinGame();
        }
    }
    public IEnumerator WinGame()
    {
        yield return new YieldWaitForSeconds(0.4f);
        WinGamePanel.SetActive(true);
    }

    public void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }

    public void ShowGrid()
    {
        float offset = gridMat.GetFloat("_IsShow");
        gridMat.SetFloat("_IsShow", offset == 1 ? 0 : 1);
        GameManager.Instance.GridOn = offset == 1 ? false : true;
    }

    internal void ShowInput(bool v)
    {
        InputPanel.SetActive(v);
    }
}
