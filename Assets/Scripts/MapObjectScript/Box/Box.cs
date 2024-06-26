using MyInputSystem;
using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Box : MapObject
{
    public BaseKESimu kESimu;

    public float MoveInterval = 0.2f;

    public BoxMaterialType boxMaterial;

    public Sprite[] sprites;


    public void Init()
    {
        int idx = kESimu.KEType switch
        {
            KEDeliverType.None => 0,
            KEDeliverType.StaticDir => 1,
            KEDeliverType.Calculate => 2,
            _ => 0,
        };
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[boxMaterial == BoxMaterialType.Wood ? idx : idx + 3];
    }

    public IEnumerator Move(Vector2 dir, Command command, bool isHit, float delta)
    {
        while (kESimu.Energe > 0)
        {
            //Debug.Log(objectId + "kESimu.Energe:" + kESimu.Energe);
            Move(dir, command, isHit);
            yield return new WaitForSeconds(delta);
        }
    }

    private void Move(Vector2 dir, Command command, bool isHit)
    {
        //Debug.Log("recieved bomb in pos:" + pos);
        Vector2 moveDir = dir;
        Vector2 movePos = WorldPos + moveDir;
        //Debug.Log(dir);
        if (MapManager.Instance.BoxCanMove(movePos, dir))
        {
            //Debug.Log("worldpos:" + WorldPos + " " + command);
            var move = new BoxMove(this, moveDir, isHit);
            command.Next.Add(move);
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.BoxMove, WorldPos, command, moveDir);
            move.Execute();
        }
        else if (isHit)
        {
            HitHandle(dir, command, isHit);
        }
    }

    public void Move(Vector2 dir, Command command)
    {
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);
    }

    public void MoveDontCalWater(Vector2 dir)
    {
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);
    }
    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        //Debug.Log(objectId + " " + kESimu.KEType);
        switch (mapEvent)
        {
            case MapEventType.Bomb:
                if (kESimu.KEType == KEDeliverType.WildCard)
                    (kESimu as WildCardKESimu).SetHitKE(new BaseKESimu(KEDeliverType.None));
                BoxBeHit boxbehit = new BoxBeHit(this, 1, this.WorldPos - happenPos);
                boxbehit.Execute();
                command.Next.Add(boxbehit);
                HitedHandle(boxbehit, this.WorldPos - happenPos, true);
                break;
            case MapEventType.PlayerMove:
            case MapEventType.BombMove:
                if (boxMaterial == BoxMaterialType.Stone) return;
                Move(this.WorldPos - happenPos, command, false);
                break;
            case MapEventType.BoxCollision:
                if (kESimu.KEType == KEDeliverType.WildCard)
                {
                    Debug.Log("WildCard objID" + command.ObjectId);
                    var hitObj = MapManager.Instance.GetMapObjectByObjID(command.ObjectId);
                    if (hitObj is Box)
                    {
                        (kESimu as WildCardKESimu).SetHitKE((hitObj as Box).kESimu);
                    }
                    else
                    {
                        Debug.LogError("err");
                    }
                }
                IHitCommand cmd = command as IHitCommand;
                if (cmd == null)
                {
                    Debug.LogError("err");
                }
                BoxBeHit boxbehit1 = new BoxBeHit(this, cmd.Energe, cmd.Dir);
                boxbehit1.Execute();
                command.Next.Add(boxbehit1);
                HitedHandle(boxbehit1, this.WorldPos - happenPos, false);
                break;
            default: break;
        }
    }

    /// <summary>
    /// 被撞之后的处理
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isBomb"></param>
    public void HitedHandle(Command command, Vector2 dir, bool isBomb)
    {
        Debug.Log("HitedHandle:" + objectId + " " + command.ObjectId + " " + kESimu.KEType);
        if (kESimu == null) return;
        switch (kESimu.KEType)
        {
            case KEDeliverType.None:
            case KEDeliverType.StaticDir:
            case KEDeliverType.ClockWise:
            case KEDeliverType.Calculate:
            case KEDeliverType.WildCard:
                StartCoroutine(Move(kESimu.Dir, command, true, MoveInterval));
                break;
            case KEDeliverType.Destory:
                this.gameObject.SetActive(false);
                break;
            case KEDeliverType.Motivate:
                Motivate(command, dir);
                break;
        }
    }
    /// <summary>
    /// 移动撞到其他物品的处理
    /// </summary>
    /// <param name="dir">移动方向</param>
    /// <param name="command"></param>
    /// <param name="isHit"></param>
    public void HitHandle(Vector2 dir, Command command, bool isHit)
    {
        //Debug.Log("HitHandle"+objectId);
        if (kESimu == null || !isHit) return;
        BoxHit cmd = new BoxHit(this, kESimu.Energe, kESimu.Dir);
        command.Next.Add(cmd);
        switch (kESimu.KEType)
        {
            case KEDeliverType.None:
            case KEDeliverType.StaticDir:
            case KEDeliverType.ClockWise:
            case KEDeliverType.Calculate:
            case KEDeliverType.WildCard:
                MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.BoxCollision,
                    WorldPos, cmd, dir);
                break;
            case KEDeliverType.Destory:
            case KEDeliverType.Motivate:
                Debug.LogError("err");
                break;
        }
        cmd.Execute();
        if (kESimu.Energe == 0)
        {
            bool isInWater = MapManager.Instance.MapObjs(ArrayPos)
                .Find(x => x.type == MapObjectType.Water) != null;
            if (isInWater)
            {
                Debug.Log("into water");
                MapObjIntoWater cmd1 = new MapObjIntoWater(this);
                command.Next.Add(cmd1);
                cmd1.Execute();
            }
        }
    }

    Vector2[] Dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public void Motivate(Command command, Vector2 dir)
    {
        Debug.Log("Motivate" + kESimu.Energe + kESimu.Dir);
        foreach (var d in Dirs)
        {
            if (d == -dir) continue;
            BoxHit hit = new BoxHit(this, kESimu.Energe, d);
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two,
                 MapEventType.BoxCollision, WorldPos, hit, d);
            command.Next.Add(hit);
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
        objectId = b.objectId;
        //distance = dis;
        if (isHit)
        {
            box.kESimu.EnergeDesc(1);
        }
    }
    public override void Execute()
    {
        box.Move(dir, this);
        //Debug.Log("Execute box Dir:" + dir);
    }

    public override void Undo()
    {
        //Debug.Log("undo box Dir:" + dir);
        box.MoveDontCalWater(-dir);
    }
}

public class BoxHit : Command, IHitCommand
{
    Box box;
    int energe;
    Vector2 dir;

    public int Energe => energe;

    public Vector2 Dir => dir;

    public BoxHit(Box box, int energe, Vector2 dir)
    {
        objectId = box.objectId;
        this.box = box;
        this.energe = energe;
        this.dir = dir;
    }

    public override void Execute()
    {
        box.kESimu.ClearEnerge();
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
        objectId = box.objectId;
        this.box = box;
        this.energe = energe;
        this.dir = dir;
    }

    public override void Execute()
    {
        box.kESimu.SetEnergeDir(energe, dir);
        //Debug.Log("after hited:" + objectId + " " + energe + " " + dir);
    }

    public override void Undo()
    {
        box.kESimu.ClearEnerge();
    }
}
