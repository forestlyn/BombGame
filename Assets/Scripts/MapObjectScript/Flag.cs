using MyInputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MapObject
{
    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.PlayerMove:
                Debug.Log("Player arrive flag!");
                break;
        }
    }
}
