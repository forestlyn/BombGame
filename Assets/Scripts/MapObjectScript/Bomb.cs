using MyInputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Bomb : MapObject
{
    public void Explosion(Command command)
    {
        Debug.Log("explosion!");
        MyEventSystem.Instance.InvokeEvent(4, MapEventType.Bomb, WorldPos, command);
    }


    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
    }
}
