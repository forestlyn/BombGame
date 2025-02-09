using MyInputSystem;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Box : MapObject
{
    public IMove uniformMove;

    public BaseKESimu kESimu;

    public float MoveInterval = 0;

    
    public float ShineTime = 0f;
    public Color ShineColor;
    public Color DefaultWoodColor;
    public Color DefaultStoneColor;
    public Color DefaultColor;
    public Material ShineMaterial;

    public BoxMaterialType boxMaterial;

    public GameObject boxMainSpriteObj;
    public GameObject boxSymbolSpriteObj;
    public Sprite[] mainsprites;
    public Sprite[] symbolsprites;

    public bool IsMoving
    {
        get => uniformMove.IsMoving || kESimu.Energe != 0;
    }

    public void Init()
    {
        boxSymbolSpriteObj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        int idx = kESimu.KEType switch
        {
            KEDeliverType.None => 0,
            KEDeliverType.StaticDir => 1,
            KEDeliverType.Calculate => 2,
            _ => 0,
        };
        if (kESimu.KEType == KEDeliverType.StaticDir)
        {
            if (kESimu.Dir == Vector2.up)
            {
                boxSymbolSpriteObj.transform.Rotate(0, 0, 90);
            }
            else if (kESimu.Dir == Vector2.left)
            {
                boxSymbolSpriteObj.transform.Rotate(0, 0, 180);
            }
            else if (kESimu.Dir == Vector2.down)
            {
                boxSymbolSpriteObj.transform.Rotate(0, 0, 270);
            }
        }
        boxMainSpriteObj.GetComponent<SpriteRenderer>().sprite = mainsprites[boxMaterial == BoxMaterialType.Wood ? 0 : 1];
        boxSymbolSpriteObj.GetComponent<SpriteRenderer>().sprite = symbolsprites[boxMaterial == BoxMaterialType.Wood ? idx : idx + 3];
        uniformMove = GetComponent<IMove>();
        uniformMove.OnSpeedBecomeZero += CheckInWater;
        ShineMaterial = boxSymbolSpriteObj.GetComponent<SpriteRenderer>().material;
        DefaultColor = boxMaterial == BoxMaterialType.Wood ? DefaultWoodColor : DefaultStoneColor;
        ShineMaterial.SetColor("_Color", DefaultColor);
        BoxManager.Add(this);
    }

    public override BaseMapObjectState MyDestory()
    {
        BoxManager.Remove(this);
        return base.MyDestory();
    }


    private Command LastestMoveCmd;
    private void CheckInWater()
    {
        if (kESimu.Energe == 0)
        {
            bool isInWater = MapManager.Instance.MapObjs(ArrayPos)?
                .Find(x => x.type == MapObjectType.Water) != null;
            if (isInWater)
            {
                Debug.Log("into water");
                MapObjIntoWater cmd1 = new MapObjIntoWater(this);
                LastestMoveCmd.Next.Add(cmd1);
                cmd1.Execute();
            }
        }
    }

    //需要hit后关闭一下，有时候会开启两个Coroutine
    public IEnumerator Move(Vector2 dir, Command command, bool isHit, float delta)
    {
        //Debug.Log(objectId + "start a coroutine" + System.DateTime.Now.ToString("HH:mm:ss.fff"));
        while (kESimu.Energe > 0)
        {
            //Debug.Log("I'm moving~ Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
            //Debug.Log(MoveInterval);
            //Debug.Log(objectId + " " + kESimu.Dir + kESimu.Energe + " " + dir + movedir + delta);
            while (uniformMove.IsMoving)
            {
                //Debug.Log("I'm waiting~ Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
                yield return new WaitForSeconds(0.001f);
            }
            uniformMove.MoveDistance = kESimu.Energe;

            Move(kESimu.Dir, command, isHit);
            //Debug.Log("I'm moving over~ Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
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
            //Debug.Log("Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
            //Debug.Log("worldpos:" + WorldPos + " " + command);
            var move = new BoxMove(this, moveDir, isHit);
            command.Next.Add(move);
            Vector2 pos = WorldPos;
            move.Execute();
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.BoxMove, pos, command, moveDir);
        }
        else if (isHit)
        {
            //Debug.Log("Hit" + isHit);
            HitHandle(dir, command, isHit);
        }
        else
        {
            Debug.LogWarning("no Hit and can't move");
        }
        if (kESimu.Energe == 0)
        {
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.One,
                MapEventType.BoxStop, WorldPos, null);
        }
    }

    public void Move(Vector2 dir, Command command)
    {
        MoveTo(WorldPos, WorldPos + dir);
        //transform.Translate(dir);
        uniformMove.Target = WorldPos + dir;
        LastestMoveCmd = command;
    }

    public void MoveDontCalWater(Vector2 dir,Command command)
    {
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);
        LastestMoveCmd = command;
    }

    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        if (command != null && command.ObjectId == objectId)
        {
            //Debug.Log(mapEvent+":event send by self");
            return;
        }

        //if (command == null)
        //{
        //    Debug.LogError(mapEvent);
        //}
        //if(command != null)
        //{
        //    Debug.Log(objectId + " " + kESimu.KEType + command.ObjectId);
        //}
        switch (mapEvent)
        {
            case MapEventType.Bomb:
                if (kESimu.KEType == KEDeliverType.WildCard)
                    (kESimu as WildCardKESimu).SetHitKE(new BaseKESimu(KEDeliverType.None));
                BoxBeHit boxbehit = new BoxBeHit(this, 1, this.WorldPos - happenPos);
                boxbehit.Execute();
                command.Next.Add(boxbehit);
                StartCoroutine(StartHitedHandle(boxbehit, this.WorldPos - happenPos, true));
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
                StartCoroutine(StartHitedHandle(boxbehit1, this.WorldPos - happenPos, false));
                break;
            default: break;
        }
    }


    public IEnumerator StartHitedHandle(Command command, Vector2 dir, bool isBomb)
    {
        ShineMaterial.SetColor("_Color", ShineColor);
        if (kESimu.KEType != KEDeliverType.None)
        {
            yield return new WaitForSeconds(ShineTime);
        }
        ShineMaterial.SetColor("_Color", DefaultColor);
        HitedHandle(command, dir, isBomb);
    }
    /// <summary>
    /// 被撞之后的处理
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isBomb"></param>
    public void HitedHandle(Command command, Vector2 dir, bool isBomb)
    {
        //Debug.Log("HitedHandle:" + objectId + " " + command.ObjectId + " " + kESimu.KEType);
        if (kESimu == null) return;
        switch (kESimu.KEType)
        {
            case KEDeliverType.None:
            case KEDeliverType.StaticDir:
            case KEDeliverType.ClockWise:
            case KEDeliverType.Calculate:
            case KEDeliverType.WildCard:
                StopAllCoroutines();
                StartCoroutine(Move(kESimu.Dir, command, true, MoveInterval));
                break;
            case KEDeliverType.Destory:
                gameObject.SetActive(false);
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
        //Debug.Log("HitHandle" + kESimu.Energe + kESimu.Dir + dir);
        command.Next.Add(cmd);
        cmd.Execute();
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
        if (kESimu.Energe == 0)
        {
            bool isInWater = MapManager.Instance.MapObjs(ArrayPos)?
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
    public BoxMove(Box b, Vector2 dir, bool isHit) : base(b)
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
        box.MoveDontCalWater(-dir,this);
    }
}

public class BoxHit : Command, IHitCommand
{
    Box box;
    int energe;
    Vector2 dir;

    public int Energe => energe;

    public Vector2 Dir => dir;

    public BoxHit(Box box, int energe, Vector2 dir) : base(box)
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
        //Debug.Log(energe + " " + objectId);
        box.kESimu.SetEnergeDir(energe, dir);
    }
}

public class BoxBeHit : Command
{
    Box box;
    public int energe;
    public Vector2 dir;

    public BoxBeHit(Box box, int energe, Vector2 dir) : base(box)
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
        //Debug.Log("undo hited:" + objectId + " " + energe + " " + dir);
    }
}
