using MyInputSystem;
using MyTools.MyCoroutines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MapObject
{

    public IMove uniformMove;

    public bool IsMoving { get => uniformMove.IsMoving; }
    private static Player instance;
    public static Player Instance { get { return instance; } }

    public int Height;
    /// <summary>
    /// 当前还可以放置炸弹数目
    /// </summary>
    public int HoldBombNum;
    public int HoldBombMaxNum;
    public float moveTimeInterval;

    public List<Bomb> bombs;
    public BaseKESimu kESimu;
    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        //Debug.Log(objectId + " " + kESimu.KEType);
        switch (mapEvent)
        {
            case MapEventType.Bomb:
                PlayerDestory cmd = new PlayerDestory(this);
                cmd.Execute();
                command.Next.Add(cmd);
                break;
            case MapEventType.BoxCollision:
                InputManager.PlayerCanInput = false;
                BoxHit cmd1 = command as BoxHit;
                if (cmd1 == null)
                {
                    Debug.LogError("err");
                }
                PlayerBeHit playerbehit = new PlayerBeHit(this, cmd1.Energe, cmd1.Dir);
                playerbehit.Execute();
                command.Next.Add(playerbehit);
                HitedHandle(playerbehit, this.WorldPos - happenPos, false);
                break;
            default: break;
        }
    }
    private void Awake()
    {
        instance = this;
        bombs = new List<Bomb>();
        kESimu = new BaseKESimu(KEDeliverType.None);
        uniformMove = GetComponent<IMove>();
        uniformMove.OnSpeedBecomeZero += CheckInWater;
    }
    private void Start()
    {
        HoldBombNum = HoldBombMaxNum;
    }
    void Update()
    {

    }

    public override BaseMapObjectState MyDestory()
    {
        InputManager.PlayerCanInput = false;
        return base.MyDestory();
    }
    public void ResetPlayer()
    {
        HoldBombNum = HoldBombMaxNum;
        bombs.Clear();
        InputManager.PlayerCanInput = true;
    }

    #region 炸弹
    public Bomb PutBomb(Vector2 pos)
    {
        var b = MyGameObjectPool.Instance.Get<Bomb>();
        b.transform.position = pos;
        HoldBombNum--;
        bombs.Add(b.GetComponent<Bomb>());
        return b.GetComponent<Bomb>();
    }
    public List<Vector2> InvokeBomb()
    {
        var bombPos = new List<Vector2>();
        foreach (var b in bombs)
        {
            bombPos.Add(b.transform.position);
            b.Explosion();
        }
        bombs.Clear();
        HoldBombNum = HoldBombMaxNum;
        return bombPos;
    }
    #endregion

    #region 移动
    public IEnumerator Move(Vector2 dir, Command command, bool isHit, float delta)
    {
        while (kESimu.Energe > 0)
        {
            //Debug.Log(objectId + "kESimu.Energe:" + kESimu.Energe);
            while (uniformMove.IsMoving)
            {
                yield return new WaitForSeconds(0.001f);
            }
            uniformMove.MoveDistance = kESimu.Energe;
            Move(dir, command, isHit);
            yield return new WaitForSeconds(delta);
        }
    }

    private void Move(Vector2 dir, Command command, bool isHit)
    {
        //Debug.Log("recieved bomb in pos:" + pos);
        //Debug.Log(dir);
        if (MapManager.Instance.BoxCanMove(WorldPos + dir, dir))
        {
            //Debug.Log("worldpos:" + WorldPos + " " + command);
            var move = new PlayerMove(this, dir, isHit);
            command.Next.Add(move);
            move.Execute();
        }
        else if (isHit)
        {
            HitHandle(dir, command, isHit);
        }
    }

    public void Move(Vector2 dir, Command command)
    {
        //Debug.Log(WorldPos);
        MoveTo(WorldPos, WorldPos + dir);
        //transform.Translate(dir);
        uniformMove.Target = WorldPos + dir;
        LastestMoveCmd = command;
    }
    public void UndoMove(Vector2 dir, Command command)
    {
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);
        //uniformMove.Target = WorldPos + dir;
        LastestMoveCmd = command;
    }

    public Command LastestMoveCmd;

    public void CheckInWater()
    {
        if (kESimu.Energe == 0)
        {
            bool isInWater = MapManager.Instance.MapObjs(ArrayPos)
                .Find(x => x.type == MapObjectType.Water) != null;
            //Debug.Log(ArrayPos);
            if (isInWater)
            {
                PlayerDestory cmd = new PlayerDestory(this);
                cmd.Execute();
                LastestMoveCmd.Next.Add(cmd);
            }
            else
            {
                InputManager.PlayerCanInput = true;
            }
        }
    }
    /// <summary>
    /// 被撞之后的处理
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isBomb"></param>
    public void HitedHandle(Command command, Vector2 dir, bool isBomb)
    {
        Debug.Log("HitedHandle:" + objectId + " " + command.ObjectId + " " + kESimu.KEType + " " + kESimu.Energe);
        if (kESimu == null) return;
        StopAllCoroutines();
        StartCoroutine(Move(kESimu.Dir, command, true, moveTimeInterval));
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
        PlayerHit cmd = new PlayerHit(this, kESimu.Energe, kESimu.Dir);
        command.Next.Add(cmd);
        cmd.Execute();
        MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.BoxCollision,
            WorldPos, cmd, dir);

        InputManager.PlayerCanInput = true;

        bool isInWater = MapManager.Instance.MapObjs(ArrayPos)
            .Find(x => x.type == MapObjectType.Water) != null;
        if (isInWater)
        {
            PlayerDestory cmd1 = new PlayerDestory(this);
            cmd1.Execute();
            cmd.Next.Add(cmd1);
        }
    }

    #endregion

    #region Undo
    public void UndoPutBomb()
    {
        if (bombs.Count > 0)
        {
            var b = bombs[bombs.Count - 1];
            bombs.RemoveAt(bombs.Count - 1);
            MyGameObjectPool.Instance.Return<Bomb>(b.gameObject);
            HoldBombNum++;
        }
    }

    internal void RemoveBomb(Bomb bomb)
    {
        if (bombs != null && bombs.Contains(bomb))
        {
            bombs.Remove(bomb);
            HoldBombNum++;
        }
    }

    #endregion
}


