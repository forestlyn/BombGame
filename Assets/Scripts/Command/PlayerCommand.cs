using MyInputSystem;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
namespace MyInputSystem
{
    public abstract class PlayerCommand : Command
    {
        protected Player player;
        public PlayerCommand(Player player)
        {
            objectId = player.objectId;
            this.player = player;
        }
    }
    public class PlayerMove : PlayerCommand
    {
        Vector2 dir;
        bool isHit;
        public PlayerMove(Player player, Vector2 dir, bool isHit) : base(player)
        {
            this.dir = dir;
            this.isHit = isHit;
            if (isHit)
            {
                player.kESimu.EnergeDesc(1);
            }
        }

        public override void Execute()
        {
            player.Move(dir, this);
        }

        public override void Undo()
        {
            player.Move(-dir, this);
        }
    }

    public class PlayerPutBomb : PlayerCommand
    {
        Vector2 bombPos;
        public PlayerPutBomb(Player player) : base(player)
        {
        }
        public override void Execute()
        {
            bombPos = player.WorldPos;
            var bomb = player.PutBomb(player.WorldPos);
            var bombState = new BaseMapObjectState(MapObjectType.Bomb, 0);
            bombState.mapObject = bomb;
            MapManager.Instance.AddNewObj(bomb.ArrayPos, bombState);
            //Debug.Log(bomb.ArrayPos);
        }

        public override void Undo()
        {
            var bombArrayPos = MapManager.CalMapPos(player.WorldPos);
            var bombState = MapManager.Instance.MapObjs(bombArrayPos).Find(o => o.type == MapObjectType.Bomb);
            if (bombState != null && bombState.mapObject != null)
            {
                var bomb = bombState.mapObject;
                bomb.MyDestory();
                bomb.transform.position = MapObject.hiddenPos;
            }
            else
            {
                Debug.LogError("can't find bomb!");
            }
        }
    }

    public class PlayerInvokeBomb : PlayerCommand
    {
        List<Vector2> bombPos;
        public PlayerInvokeBomb(Player player) : base(player)
        {
        }

        public override void Execute()
        {
            bombPos = player.InvokeBomb();
        }

        public override void Undo()
        {
            foreach (var pos in bombPos)
            {
                var bomb = player.PutBomb(pos);
                var bombState = new BaseMapObjectState(MapObjectType.Bomb, 0);
                bombState.mapObject = bomb;
                MapManager.Instance.AddNewObj(bomb.ArrayPos, bombState);
            }
        }
    }
}

public class PlayerHit : PlayerCommand, IHitCommand
{
    int energe;
    Vector2 dir;

    public int Energe => energe;

    public Vector2 Dir => dir;
    public PlayerHit(Player player, int energe, Vector2 dir) : base(player)
    {
        this.energe = energe;
        this.dir = dir;
    }



    public override void Execute()
    {
        player.kESimu.ClearEnerge();
    }

    public override void Undo()
    {
        player.kESimu.SetEnergeDir(energe, dir);
    }
}

public class PlayerBeHit : PlayerCommand
{
    public int energe;
    public Vector2 dir;

    public PlayerBeHit(Player player, int energe, Vector2 dir) : base(player)
    {
        this.energe = energe;
        this.dir = dir;
    }

    public override void Execute()
    {
        InputManager.PlayerCanInput = false;
        player.kESimu.SetEnergeDir(energe, dir);
        //Debug.Log("after hited:" + objectId + " " + energe + " " + dir);
    }

    public override void Undo()
    {
        player.kESimu.ClearEnerge();
    }
}

public class PlayerDestory : PlayerCommand
{
    Vector2 worldPos;
    Vector2 arrayPos;
    BaseMapObjectState state;
    public PlayerDestory(Player player) : base(player)
    {
        worldPos = player.WorldPos;
        arrayPos = player.ArrayPos;
    }
    public override void Execute()
    {
        state = player.MyDestory();
        player.transform.position = MapObject.hiddenPos;

    }

    public override void Undo()
    {
        MapManager.Instance.MapObjs(arrayPos).Add(state);
        player.transform.position = worldPos;
        InputManager.PlayerCanInput = true;
    }
}