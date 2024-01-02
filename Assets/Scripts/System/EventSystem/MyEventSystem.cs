using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void PressBoardChangeState(bool open, int id);

public class MyEventSystem : MonoBehaviour
{
    private static MyEventSystem instance;
    public static MyEventSystem Instance { get { return instance; } }
    public event PressBoardChangeState OnPressBoardStateChange;

    private void Awake()
    {
        instance = this;
    }

    public void InvokePressBoardStateChange(bool open,int id)
    {
        OnPressBoardStateChange?.Invoke(open, id);
    }

    public void InvokeEvent(InvokeEventType v, MapEventType mapEvent, Vector2 worldPos, Command command, Vector2 dir = default, int id = 0)
    {
        switch(v)
        {
            case InvokeEventType.Two:
                InvokeEventInTwo(mapEvent, worldPos, dir, command);
                break;
            case InvokeEventType.Four:
                InvokeEventInFour(mapEvent, worldPos, command);
                break;
            case InvokeEventType.AllId:
                InvokeEventInAllId(mapEvent, worldPos, command, id);
                break;
        }
    }

    private void InvokeEventInAllId(MapEventType mapEvent, Vector2 worldPos, Command command,int id)
    {
        MapManager.Instance.InvokeEventAllId(mapEvent, worldPos, command, id);
    }

    public void InvokeEventInTwo(MapEventType mapEvent, Vector2 worldPos,Vector2 dir, Command command)
    {
        var arrayPos = MapManager.CalMapPos(worldPos);
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

    private void InvokeEvent(MapEventType mapEvent,Vector2 arrayPos,Vector2 worldPos, Command command)
    {
        MapManager.Instance.InvokeEvent(mapEvent, arrayPos, worldPos, command);
    }
}
