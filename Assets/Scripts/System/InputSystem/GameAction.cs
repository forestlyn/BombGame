using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyInputSystem
{
    public class GameAction
    {
        public void Restart()
        {
            GameReStartCommand gameReStartCommand = new GameReStartCommand(MapManager.Instance);
            gameReStartCommand.Execute();
            RedoManager.Instance.AddCommand(gameReStartCommand);
            RedoManager.Instance.ClearCommandLists();
        }
    }
}

