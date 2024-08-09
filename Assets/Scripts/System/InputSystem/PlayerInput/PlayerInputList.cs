using System;
using System.Collections.Generic;
using UnityEngine;
namespace MyInputSystem
{
    public enum PlayerInputType
    {
        Move,
        Bomb,
        Redo,
        Undo,
        Restart
    }

    public class PlayerInput
    {
        public PlayerInputType type;
        public Vector2 dir;
        public DateTime InputTime;

        public PlayerInput(PlayerInputType type, Vector2 dir)
        {
            this.type = type;
            this.dir = dir;
            InputTime = DateTime.UtcNow;
        }
        public PlayerInput(PlayerInputType type)
        {
            this.type = type;
            InputTime = DateTime.UtcNow;
        }
    }

    public class PlayerInputList
    {
        public List<PlayerInput> playerInputList;

        public PlayerInputList()
        {
            playerInputList = new List<PlayerInput>();
        }

        public void Add(PlayerInput playerInput)
        {
            switch (playerInput.type)
            {
                case PlayerInputType.Move:
                case PlayerInputType.Bomb:
                    break;
                case PlayerInputType.Redo:
                case PlayerInputType.Undo:
                case PlayerInputType.Restart:
                    playerInputList.Clear();
                    break;
            }
            playerInputList.Add(playerInput);
        }

        public PlayerInput GetNext()
        {
            UpdateList();
            if (playerInputList.Count > 0)
            {
                var playerInput = playerInputList[0];
                playerInputList.RemoveAt(0);
                return playerInput;
            }
            return null;
        }

        private void UpdateList()
        {
            DateTime time = DateTime.UtcNow;
            for (int i = 0; i < playerInputList.Count; i++)
            {
                PlayerInput playerInput = playerInputList[i];
                TimeSpan timeDifference = time - playerInput.InputTime;
                //Debug.Log(timeDifference.ToString());
                if (timeDifference.TotalSeconds >= 0.3)
                {
                    playerInputList.Remove(playerInput);
                }
            }
        }
    }
}