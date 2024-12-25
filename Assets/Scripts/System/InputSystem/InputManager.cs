using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MyInputSystem
{
    public class InputManager : MonoBehaviour
    {
        private IInputAction inputActions;
        private PlayerAction playerAction;
        private GameAction gameAction;

        public bool ShowplayerCanInput = true;
        [SerializeField]
        private static bool playerCanInput = true;

        public static bool PlayerCanInput
        {
            get { return playerCanInput; }
            set
            {
                //Debug.LogWarning("playerCanInput:" + playerCanInput);
                if(playerCanInput != value)
                {
                    playerCanInput = value;
                    if (playerCanInput)
                    {
                        MapManager.Instance.CheckGameState();
                    }
                }
            }
        }
        private void Update()
        {
            ShowplayerCanInput = playerCanInput;
        }


        private void Awake()
        {
            inputActions = new UnityInputAction();
        }
        void Start()
        {
            playerAction = new PlayerAction(Player.Instance);
            gameAction = new GameAction();
            BindActionFunc();
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }
        private void BindActionFunc()
        {
            BindPlayerInput();
            BindGameInput();
        }

        private void BindGameInput()
        {
            inputActions.Redo.performed += cbContext =>
            {
                gameAction.Redo();
            };
            inputActions.Undo.started += cbContext =>
            {
                gameAction.Undo();
            };
            inputActions.Undo.performed += cbContext =>
            {
                gameAction.Undo(cbContext);
            };
            inputActions.Undo.canceled += cbContext =>
            {
                gameAction.StopUndo(cbContext);
            };
            inputActions.ReStart.performed += cbContext =>
            {
                gameAction.Restart();
            };
            inputActions.ShowGrid.performed += cbContext =>
            {
                gameAction.ShowGrid();
            };
        }
        private void BindPlayerInput()
        {
            //player move
            inputActions.MoveUp.started += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.up);
                }
            };
            inputActions.MoveUp.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.up, cbContext);
                }
            };
            inputActions.MoveUp.canceled += cbContext =>
            {
                if (PlayerCanInput)
                    playerAction.StopMove(Vector2.up, cbContext);
            };

            inputActions.MoveDown.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.down);
                }
            };
            inputActions.MoveDown.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.down, cbContext);
                }
            };
            inputActions.MoveDown.canceled += cbContext =>
            {
                if (PlayerCanInput)
                    playerAction.StopMove(Vector2.down, cbContext);
            };

            inputActions.MoveLeft.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.left);
                }
            };
            inputActions.MoveLeft.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.left, cbContext);
                }
            };
            inputActions.MoveLeft.canceled += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.StopMove(Vector2.left, cbContext);
                }
            };

            inputActions.MoveRight.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.right);
                }
            };
            inputActions.MoveRight.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.right, cbContext);
                }
            };
            inputActions.MoveRight.canceled += cbContext =>
            {
                if (PlayerCanInput)
                    playerAction.StopMove(Vector2.right, cbContext);
            };

            inputActions.Bomb.performed += cbContext =>
            {
                //Debug.Log("Bomb PlayerCanInput" + PlayerCanInput);
                if (PlayerCanInput)
                    playerAction.Bomb();
            };
        }
    }

}
