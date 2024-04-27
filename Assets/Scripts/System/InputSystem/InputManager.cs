using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyInputSystem
{
    public class InputManager : MonoBehaviour
    {
        private PCInputActions pcInputActions;
        private PlayerAction playerAction;
        private GameAction gameAction;
        private void Awake()
        {
            pcInputActions = new PCInputActions();
        }
        void Start()
        {
            playerAction = new PlayerAction(Player.Instance);
            gameAction = new GameAction();
            BindActionFunc();
        }
        private void OnEnable()
        {
            pcInputActions.Enable();
        }
        private void OnDisable()
        {
            pcInputActions.Disable();
        }
        private void BindActionFunc()
        {
            BindPlayerInput();
            BindGameInput();
        }
        private void BindGameInput()
        {
            pcInputActions.Game.Redo.performed += cbContext =>
            {
                RedoManager.Instance.Redo();
            };
            pcInputActions.Game.Undo.performed += cbContext =>
            {
                RedoManager.Instance.Undo();
            };
            pcInputActions.Game.ReStart.performed += cbContext =>
            {
                gameAction.Restart();
            };
        }
        private void BindPlayerInput()
        {
            //player move
            pcInputActions.Player.MoveUp.performed += cbContext =>
            {
                playerAction.Move(Vector2.up, cbContext);
            };
            pcInputActions.Player.MoveUp.canceled += cbContext =>
            {
                playerAction.StopMove(Vector2.up, cbContext);
            };

            pcInputActions.Player.MoveDown.performed += cbContext =>
            {
                playerAction.Move(Vector2.down, cbContext);
            };
            pcInputActions.Player.MoveDown.canceled += cbContext =>
            {
                playerAction.StopMove(Vector2.down, cbContext);
            };

            pcInputActions.Player.MoveLeft.performed += cbContext =>
            {
                playerAction.Move(Vector2.left, cbContext);
            };
            pcInputActions.Player.MoveLeft.canceled += cbContext =>
            {
                playerAction.StopMove(Vector2.left, cbContext);
            };

            pcInputActions.Player.MoveRight.performed += cbContext =>
            {
                playerAction.Move(Vector2.right, cbContext);
            };
            pcInputActions.Player.MoveRight.canceled += cbContext =>
            {
                playerAction.StopMove(Vector2.right, cbContext);
            };

            pcInputActions.Player.Bomb.performed += cbContext =>
            {
                playerAction.Bomb();
            };
        }
    }

}
