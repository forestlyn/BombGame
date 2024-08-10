using MyTools.MyCoroutines;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
            //Debug.Log(cbContext.interaction is HoldInteraction);
            if (cbContext.interaction is HoldInteraction)
            {
                StopMove();
                move = MyCoroutines.StartCoroutine(StartContinuousMove(dir));
            }
            if (cbContext.interaction is TapInteraction)
            {
                StopMove();
            }
        }


        internal void StopMove(Vector2 dir, InputAction.CallbackContext cbContext)
        {
            if (cbContext.interaction is HoldInteraction)
            {
                StopMove();
            }
        }

        //public void Bomb()
        //{
        //    Command cmd;
        //    if (player.HoldBombNum > 0)
        //    {
        //        cmd = new PlayerPutBomb(player);
        //        cmd.Execute();
        //    }
        //    else
        //    {
        //        if (MapManager.Instance.CheckType(player.ArrayPos, MapObjectType.Bomb))
        //            return;
        //        cmd = new PlayerInvokeBomb(player);
        //        var bombs = new List<Bomb>(player.bombs);
        //        cmd.Execute();
        //        foreach (var b in bombs)
        //        {
        //            MyEventSystem.Instance.InvokeEvent(InvokeEventType.Four, MapEventType.Bomb, b.WorldPos, cmd);
        //        }
        //    }
        //    RedoManager.Instance.AddCommand(cmd);
        //}

        //private void Move(Vector2 dir)
        //{
        //    if (!MapManager.Instance.PlayerCanMove(player.WorldPos, dir))
        //        return;
        //    if (MapManager.Instance.CheckType(MapManager.CalMapPos(player.WorldPos + dir), MapObjectType.Water))
        //        return;

        //    Command cmd = new PlayerMove(player, dir, false);
        //    Vector2 pos = player.WorldPos;
        //    cmd.Execute();
        //    MyEventSystem.Instance.InvokeEvent(InvokeEventType.Two, MapEventType.PlayerMove, pos, cmd, dir);
        //    RedoManager.Instance.AddCommand(cmd);
        //}
        private IEnumerator StartContinuousMove(Vector2 dir)
        {
            while (true)
            {
                Move(dir);
                yield return new YieldWaitForSeconds(player.moveTimeInterval);
            }
        }

        internal void Move(Vector2 dir)
        {
            Debug.Log("1");
            var input = new PlayerInput(PlayerInputType.Move, dir); ;
            PlayerInputManager.Instance.Add(input);
        }

        private void StopMove()
        {
            MyCoroutines.StopCoroutine(move);
        }

        internal void Bomb()
        {
            var input = new PlayerInput(PlayerInputType.Bomb);
            PlayerInputManager.Instance.Add(input);
        }
    }
}

