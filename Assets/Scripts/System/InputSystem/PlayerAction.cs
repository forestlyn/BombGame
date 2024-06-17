using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using MyTools.MyCoroutines;
using System;
using Unity.VisualScripting;

namespace MyInputSystem
{
    public class PlayerAction
    {
        Player player;
        MyCoroutine move;

        public PlayerAction(Player player)
        {
            this.player = player;
        }

        internal void Move(Vector2 dir, InputAction.CallbackContext cbContext)
        {
            if (cbContext.interaction is HoldInteraction)
            {
                StopMove();
                move = MyCoroutines.StartCoroutine(StartContinuousMove(dir));
            }
            if (cbContext.interaction is TapInteraction)
            {
                StopMove();
                Move(dir);
            }
        }


        internal void StopMove(Vector2 dir, InputAction.CallbackContext cbContext)
        {
            if (cbContext.interaction is HoldInteraction)
            {
                StopMove();
            }
        }

        public void Bomb()
        {
            Command cmd;
            if (player.HoldBombNum > 0)
            {
                cmd = new PlayerPutBomb(player);
                cmd.Execute();
            }
            else
            {
                if (MapManager.Instance.CheckType(player.ArrayPos, MapObjectType.Bomb))
                    return;
                cmd = new PlayerInvokeBomb(player);
                var bombs = new List<Bomb>(player.bombs);
                cmd.Execute();
                foreach (var b in bombs)
                {
                    MyEventSystem.Instance.InvokeEvent(InvokeEventType.Four, MapEventType.Bomb, b.WorldPos, cmd);
                }
            }
            RedoManager.Instance.AddCommand(cmd);
        }

        private void Move(Vector2 dir)
        {
            if (!MapManager.Instance.PlayerCanMove(player.WorldPos, dir))
                return;
            if (MapManager.Instance.CheckType(MapManager.CalMapPos(player.WorldPos + dir), MapObjectType.Water))
                return;

            Command cmd = new PlayerMove(player, dir, false);
            Vector2 pos = player.WorldPos;
            cmd.Execute();
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.PlayerMove, pos, cmd, dir);
            RedoManager.Instance.AddCommand(cmd);
        }

        private IEnumerator StartContinuousMove(Vector2 dir)
        {
            while (true)
            {
                Move(dir);
                yield return new YieldWaitForSeconds(player.moveTimeInterval);
            }
        }
        private void StopMove()
        {
            MyCoroutines.StopCoroutine(move);
        }
    }
}

