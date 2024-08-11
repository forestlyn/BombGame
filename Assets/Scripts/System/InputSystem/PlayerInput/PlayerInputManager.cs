using System;
using System.Collections.Generic;
using UnityEngine;
namespace MyInputSystem
{
    public class PlayerInputManager : MonoBehaviour
    {


        private PlayerInputList m_PlayerInputList;

        private static PlayerInputManager instance;
        public static PlayerInputManager Instance { get { return instance; } }

        private Player player;
        private void Awake()
        {
            player = Player.Instance;
            m_PlayerInputList = new PlayerInputList();
            if (instance == null)
                instance = this;
            else
            {
                Debug.LogError("err");
            }
        }

        private void Update()
        {
            if (Player.Instance.IsMoving || GameManager.Instance.isGameWin)
            {
                return;
            }
            var playerInput = m_PlayerInputList.GetNext();
            if (playerInput != null)
            {
                switch (playerInput.type)
                {
                    case PlayerInputType.Move:
                        Move(playerInput.dir);
                        break;
                    case PlayerInputType.Bomb:
                        Bomb();
                        break;
                    case PlayerInputType.Redo:
                        Redo();
                        break;
                    case PlayerInputType.Undo:
                        Undo();
                        break;
                    case PlayerInputType.Restart:
                        Restart();
                        break;
                }
            }
        }

        public void ClearInputList()
        {
            m_PlayerInputList.Clear();
        }

        internal void Add(PlayerInput input)
        {
            m_PlayerInputList.Add(input);
        }

        private void Bomb()
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

        private void Restart()
        {
            GameReStartCommand gameReStartCommand = new GameReStartCommand(MapManager.Instance);
            gameReStartCommand.Execute();
            RedoManager.Instance.AddCommand(gameReStartCommand);
            RedoManager.Instance.ClearCommandLists();
        }
        private void Redo()
        {
            RedoManager.Instance.Redo();
        }

        private void Undo()
        {
            RedoManager.Instance.Undo();
        }
    }
}
