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
        if (MapManager.InRange(pos, WorldPos, 4)
            && MapManager.Instance.BoxCanMove(movePos))
        {
            MapManager.Instance.SwapMapObjByWorldPos(WorldPos, movePos);
            Debug.Log("in range 4 worldpos:" + WorldPos);
            var move = new BoxMove(this, moveDir);
            move.Execute();
            command.Next.Add(move);
        }
    }

    public void Move(Vector2 dir)
    {
        Debug.Log("move dir :" + dir);
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
        box.Move(-dir);
    }
}