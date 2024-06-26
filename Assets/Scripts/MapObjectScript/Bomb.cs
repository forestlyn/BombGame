using MyInputSystem;
using UnityEngine;

public class Bomb : MapObject
{
    public void Explosion()
    {
        //Debug.Log("explosion!");
        MapManager.Instance.RemoveMapObj(this);
        MyGameObjectPool.Instance.Return<Bomb>(gameObject);
    }


    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.PlayerMove:
            case MapEventType.BoxMove:
                //Debug.Log("BoxMove PlayerMove:" + mapEvent + WorldPos + happenPos);
                Move(this.WorldPos - happenPos, command, false);
                break;
        }

    }
    private void Move(Vector2 dir, Command command, bool isHit)
    {
        //Debug.Log("recieved bomb in pos:" + pos);
        Vector2 moveDir = dir;
        Vector2 movePos = WorldPos + moveDir;
        //Debug.Log(dir);
        if (MapManager.Instance.BombCanMove(movePos, dir))
        {
            //Debug.Log("worldpos:" + WorldPos + " " + command);
            var move = new BombMove(this, moveDir);
            command.Next.Add(move);
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.BombMove, WorldPos, move, dir);
            move.Execute();
        }
    }
    public void Move(Vector2 dir, Command command)
    {
        //Debug.Log("move Dir :" + Dir);
        MoveTo(WorldPos, WorldPos + dir);
        transform.Translate(dir);

        bool isInWater = MapManager.Instance.MapObjs(ArrayPos)
            .Find(x => x.type == MapObjectType.Water) != null;
        if (isInWater)
        {
            MapObjIntoWater cmd = new MapObjIntoWater(this);
            cmd.Execute();
            command.Next.Add(cmd);
        }
    }

    public override BaseMapObjectState MyDestory()
    {
        Player.Instance.RemoveBomb(this);
        return base.MyDestory();
    }
}

public class BombMove : Command
{
    Bomb bomb;
    Vector2 dir;

    public BombMove(Bomb bomb, Vector2 dir)
    {
        this.bomb = bomb;
        this.dir = dir;
    }

    public override void Execute()
    {
        bomb.Move(dir, this);
    }

    public override void Undo()
    {
        bomb.Move(-dir, this);
    }
}

