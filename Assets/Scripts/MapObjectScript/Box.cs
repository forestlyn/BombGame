using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MapObject
{
    private void Move(Vector2 pos, Command command)
    {
        Debug.Log("recieved bomb in pos:" + pos);
        Vector2 moveDir = WorldPos - pos;
        Vector2 movePos = WorldPos + moveDir;
        if (MapManager.Instance.BoxCanMove(movePos))
        {
            Debug.Log("worldpos:" + WorldPos + " " + command);
            var move = new BoxMove(this, moveDir);
            command.Next.Add(move);
            move.Execute();
        }
    }

    public void Move(Vector2 dir)
    {
        Debug.Log("move dir :" + dir);
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);
    }

    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.Bomb:
            case MapEventType.PlayerMove:
                Move(happenPos, command); 
                break;
            default: break;
        }
    }
}


public class BoxMove : Command
{
    Box box;
    Vector2 dir;
    public BoxMove(Box b, Vector2 dir)
    {
        box = b;
        this.dir = dir;
    }
    public override void Execute()
    {
        box.Move(dir);
    }

    public override void Undo()
    {
        Debug.Log("undo box dir:" + dir);
        box.Move(-dir);
    }
}