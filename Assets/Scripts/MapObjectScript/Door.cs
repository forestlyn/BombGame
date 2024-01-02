using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MapObject
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] doorSprite;
    public bool Open
    {
        get => open;
        set
        {
            if (open != value)
            {
                //Debug.Log("door is open?" + value);
            }
            open = value;
            spriteRenderer.sprite = doorSprite[open ? 0 : 1];
        }
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        Open = open;
    }
    private void OnEnable()
    {
        MyEventSystem.Instance.OnPressBoardStateChange += Check;
    }
    private void OnDisable()
    {
        MyEventSystem.Instance.OnPressBoardStateChange -= Check;
    }
    private void Check(bool open, int id)
    {
        Debug.Log("id id" + id + " " + id);
        if(this.id == id)
        {
            OpenDoor(!this.open);
        }
    }

    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {

    }

    //private void OpenDoor(bool open,Command command)
    //{
    //    var cmd = new DoorOpenOrClose(this, open);
    //    command.Next.Add(cmd);
    //    cmd.Execute();
    //}

    public void OpenDoor(bool open)
    {
        Open = open;
    }
}

//public class DoorOpenOrClose : Command
//{
//    Door door;
//    bool open;
//    public DoorOpenOrClose(Door door, bool open)
//    { 
//        this.door = door; 
//        this.open = open;
//    }
//    public override void Execute()
//    {
//        door.OpenDoor(open);
//    }

//    public override void Undo()
//    {
//        door.OpenDoor(!open);
//    }
//}