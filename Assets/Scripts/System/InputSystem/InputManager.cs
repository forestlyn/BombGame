using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyInputSystem
{
    public class InputManager : MonoBehaviour
    {
        private PCInputActions pcInputActions;
        private PlayerAction playerAction;
        private void Awake()
        {
            pcInputActions = new PCInputActions();
            playerAction = new PlayerAction();
        }
        void Start()
        {
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
                //Player.Instance.Redo();
            };
            pcInputActions.Game.Undo.performed += cbContext =>
            {
                RedoManager.Instance.Undo();
                //Player.Instance.Undo();
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
