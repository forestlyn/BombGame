using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyEventSystem : MonoBehaviour
{
    private static MyEventSystem instance;
    public static MyEventSystem Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    public void InvokeEvent(int v, MapEventType mapEvent, Vector2 worldPos, Command command, Vector2 dir = default)
    {
        switch(v)
        {
            case 2:
                InvokeEventInTwo(mapEvent, worldPos, dir, command);
                break;
            case 4:
                InvokeEventInFour(mapEvent, worldPos, command);
                break;
        }
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
