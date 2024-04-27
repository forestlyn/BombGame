using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Box : MapObject
{
    public BaseKESimu kESimu;

    public float MoveInterval = 0.2f;

    public BoxMaterialType boxMaterial;

    public IEnumerator Move(Vector2 dir, Command command, bool isHit, float delta)
    {
        while (kESimu.Energe > 0)
        {
            Move(dir, command, isHit);
            yield return new WaitForSeconds(delta);
        }
    }

    private void Move(Vector2 dir, Command command, bool isHit)
    {
        //Debug.Log("recieved bomb in pos:" + pos);
        Vector2 moveDir = dir;
        Vector2 movePos = WorldPos + moveDir;
        if (MapManager.Instance.BoxCanMove(movePos))
        {
            //Debug.Log("worldpos:" + WorldPos + " " + command);
            var move = new BoxMove(this, moveDir, isHit);
            command.Next.Add(move);
            move.Execute();
        }
        else if (isHit)
        {
            List<BaseMapObjectState> objs = MapManager.Instance.MapObjs(movePos);
            Box box = objs.Find(x => x.type == MapObjectType.Box).mapObject as Box;
            if (box != null)
            {
                BoxHit boxHit = new BoxHit(this, kESimu.Energe, kESimu.Dir);
                kESimu.ClearEnergeDir();

                if (kESimu.KEType == KEDeliverType.Motivate)
                {
                    MyEventSystem.Instance.InvokeEvent(InvokeEventType.Four,
                        MapEventType.BoxCollision, WorldPos, boxHit);
                }
            }

        }
    }

    public void Move(Vector2 dir)
    {
        //Debug.Log("move Dir :" + Dir);
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);
    }

    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.Bomb:
                BoxBeHit boxbehit = new BoxBeHit(this, 1, happenPos - this.WorldPos);
                boxbehit.Execute();
                command.Next.Add(boxbehit);
                StartCoroutine(Move(kESimu.Dir, command, true, MoveInterval));
                HitedHandle(command, true);
                break;
            case MapEventType.PlayerMove:
                if (boxMaterial == BoxMaterialType.Stone) return;
                StartCoroutine(Move(happenPos, command, false, MoveInterval));
                break;
            case MapEventType.BoxCollision:
                BoxHit cmd = command as BoxHit;
                BoxBeHit boxbehit1 = new BoxBeHit(this, cmd.energe, cmd.dir);
                boxbehit1.Execute();
                command.Next.Add(boxbehit1);
                HitedHandle(command, false);
                break;
            default: break;
        }
    }

    /// <summary>
    /// 被撞之后的处理
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isBomb"></param>
    public void HitedHandle(Command command,bool isBomb)
    {
        StartCoroutine(Move(kESimu.Dir, command, true, MoveInterval));

        if (kESimu == null) return;
        switch (kESimu.KEType)
        {
            case KEDeliverType.None: 
            case KEDeliverType.StaticDir:
            case KEDeliverType.ClockWise:
            case KEDeliverType.Calculate:
                StartCoroutine(Move(kESimu.Dir, command, true, MoveInterval));
                break;
            case KEDeliverType.Destory:

                break;
            case KEDeliverType.Motivate:
                break;
        }
    }
}


public class BoxMove : Command
{
    Box box;
    Vector2 dir;
    public BoxMove(Box b, Vector2 dir, bool isHit)
    {
        box = b;
        this.dir = dir;
        //distance = dis;
        if (isHit)
        {
            box.kESimu.EnergeDesc(1);
        }
    }
    public override void Execute()
    {
        box.Move(dir);
    }

    public override void Undo()
    {
        Debug.Log("undo box Dir:" + dir);
        box.Move(-dir);
    }
}

public class BoxHit : Command
{
    Box box;
    public int energe;
    public Vector2 dir;

    public BoxHit(Box box, int energe, Vector2 dir)
    {
        this.box = box;
        this.energe = energe;
        this.dir = dir;
    }

    public override void Execute()
    {
        box.kESimu.ClearEnergeDir();
    }

    public override void Undo()
    {
        box.kESimu.SetEnergeDir(energe, dir);
    }
}

public class BoxBeHit : Command
{
    Box box;
    public int energe;
    public Vector2 dir;

    public BoxBeHit(Box box, int energe, Vector2 dir)
    {
        this.box = box;
        this.energe = energe;
        this.dir = dir;
    }

    public override void Execute()
    {
        box.kESimu.SetEnergeDir(energe, dir);
    }

    public override void Undo()
    {
        box.kESimu.ClearEnergeDir();
    }
}