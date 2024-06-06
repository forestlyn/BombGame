using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void BoxTargetChangeState(bool open, int objId);
public delegate void FlagChangeState(bool open, int objId);

public class MyEventSystem : MonoBehaviour
{
    private static MyEventSystem instance;
    public static MyEventSystem Instance { get { return instance; } }
    public event BoxTargetChangeState OnBoxTargetStateChange;
    public event FlagChangeState OnFlagStateChange;

    private void Awake()
    {
        instance = this;
    }

    public void InvokeBoxTargetStateChange(bool open,int objId)
    {
        OnBoxTargetStateChange?.Invoke(open,objId);
    }
    public void InvokeFlagStateChange(bool open, int objId)
    {
        OnFlagStateChange?.Invoke(open, objId);
    }

    public void InvokeEvent(InvokeEventType v, MapEventType mapEvent, Vector2 worldPos, Command command, Vector2 dir = default, int id = 0)
    {
        switch(v)
        {
            case InvokeEventType.One:
                InvokeEvent(mapEvent, MapManager.CalMapPos(worldPos), worldPos, command);
                break;
            case InvokeEventType.Two:
                InvokeEventInTwo(mapEvent, worldPos, dir, command);
                break;
            case InvokeEventType.Three:
                InvokeEventInThree(mapEvent, worldPos, dir, command);
                break;
            case InvokeEventType.Four:
                InvokeEventInFour(mapEvent, worldPos, command);
                break;
            case InvokeEventType.AllId:
                InvokeEventInAllId(mapEvent, worldPos, command, id);
                break;
        }
    }

    public void InvokeEventInThree(MapEventType mapEvent, Vector2 worldPos, Vector2 dir, Command command)
    {
        var arrayPos = MapManager.CalMapPos(worldPos);
        if (dir != Vector2.down)
            InvokeEvent(mapEvent, new Vector2(arrayPos.x, arrayPos.y + 1), worldPos, command);
        if (dir != Vector2.up)
            InvokeEvent(mapEvent, new Vector2(arrayPos.x, arrayPos.y - 1), worldPos, command);
        if (dir != Vector2.left) 
            InvokeEvent(mapEvent, new Vector2(arrayPos.x + 1, arrayPos.y), worldPos, command);
        if (dir != Vector2.right)
            InvokeEvent(mapEvent, new Vector2(arrayPos.x - 1, arrayPos.y), worldPos, command);
    }

    private void InvokeEventInAllId(MapEventType mapEvent, Vector2 worldPos, Command command,int id)
    {
        MapManager.Instance.InvokeEventAllId(mapEvent, worldPos, command, id);
    }

    public void InvokeEventInTwo(MapEventType mapEvent, Vector2 worldPos,Vector2 dir, Command command)
    {
        var arrayPos = MapManager.CalMapPos(worldPos);
        //Debug.Log(arrayPos +" "+ dir + " " + mapEvent);
        InvokeEvent(mapEvent, arrayPos + dir, worldPos, command);
    }

    public void InvokeEventInFour(MapEventType mapEvent, Vector2 worldPos, Command command)
    {
        var arrayPos = MapManager.CalMapPos(worldPos);
        InvokeEvent(mapEvent, new Vector2(arrayPos.x, arrayPos.y - 1), worldPos, command);
        InvokeEvent(mapEvent, new Vector2(arrayPos.x, arrayPos.y + 1), worldPos, command);
        InvokeEvent(mapEvent, new Vector2(arrayPos.x - 1, arrayPos.y), worldPos, command);
        InvokeEvent(mapEvent, new Vector2(arrayPos.x + 1, arrayPos.y), worldPos, command);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapEvent">事件类型</param>
    /// <param name="arrayPos">事件发送给当前地图哪一格子的物体</param>
    /// <param name="worldPos">事件发生的地点</param>
    /// <param name="command">指令</param>
    private void InvokeEvent(MapEventType mapEvent,Vector2 arrayPos,Vector2 worldPos, Command command)
    {
        MapManager.Instance.InvokeEvent(mapEvent, arrayPos, worldPos, command);
    }
}
