using MyInputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flag : MapObject
{
    public bool Open
    {
        get => open;
        set
        {
            if (open != value)
            {
                open = value;
                MyEventSystem.Instance.InvokeFlagStateChange(value, objectId);
                Debug.Log("flag state becomes " + value);
            }
        }
    }
    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.Arrive:
                Check(ArrayPos);
                break;
            case MapEventType.Leave:
                Check(ArrayPos);
                break;
        }
    }
    private void Check(Vector2 arrayPos)
    {
        //Debug.Log("check");
        var list = MapManager.Instance.MapObjs(arrayPos);
        foreach (var item in list)
        {
            switch (item.type)
            {
                case MapObjectType.Player:
                    Open = true;
                    return;
                default:
                    break;
            }
        }
        Open = false;
    }
}
