using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using MyTools.MyCoroutines;
using System;

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
                cmd = new PlayerPutBomb(player);
            else 
                cmd = new PlayerInvokeBomb(player);
            cmd.Execute();
            RedoManager.Instance.AddCommand(cmd);
        }

        private void Move(Vector2 dir)
        {
            if (!MapManager.Instance.PlayerCanMove(player.WorldPos, dir))
                return;
            Command cmd = new PlayerMove(player, dir);
            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.PlayerMove, player.WorldPos, cmd, dir);
            cmd.Execute();
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

