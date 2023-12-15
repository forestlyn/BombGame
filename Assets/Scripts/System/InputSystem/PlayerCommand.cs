using System.Collections.Generic;
using UnityEngine;
namespace MyInputSystem
{
    public abstract class PlayerCommand : Command
    {
        protected Player player;
        public PlayerCommand(Player player)
        {
            this.player = player;
        }
    }
    public class PlayerMove : PlayerCommand
    {
        Vector2 dir;
        public PlayerMove(Player player,Vector2 dir):base(player) {
            this.dir = dir;
        }

        public override void Execute()
        {
            player.Move(dir);
        }

        public override void Undo()
        {
            player.Move(-dir);
        }
    }

    public class PlayerBomb : PlayerCommand
    {
        PlayerCommand executeCommand;
        public PlayerBomb(Player player):base(player)
        {
            if (player.HoldBombNum > 0)
            {
                executeCommand = new PlayerPutBomb(player);
            }
            else
            {
                executeCommand = new PlayerInvokeBomb(player);
            }
        }

        public override void Execute()
        {
            executeCommand.Execute();
        }

        public override void Undo()
        {
            executeCommand.Undo();
        }
    }

    public class PlayerPutBomb : PlayerCommand
    {
        public PlayerPutBomb(Player player) : base(player)
        {
        }
        public override void Execute()
        {
           player.PutBomb(player.WorldPos);
        }

        public override void Undo()
        {
            player.UndoPutBomb();
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
           bombPos = player.InvokeBomb(this);
        }

        public override void Undo()
        {
            foreach(var pos in bombPos)
            {
                player.PutBomb(pos);
            }
        }
    }
}