using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyInputSystem
{
    public class InputManager : MonoBehaviour
    {
        private PCInputActions pcInputActions;
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
                gameAction.Redo();
            };
            pcInputActions.Game.Undo.performed += cbContext =>
            {
                gameAction.Undo();
            };
            pcInputActions.Game.ReStart.performed += cbContext =>
            {
                gameAction.Restart();
            };
        }
        private void BindPlayerInput()
        {
            //player move
            pcInputActions.Player.MoveUp.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.up);
                }
            };
            pcInputActions.Player.MoveUp.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.up, cbContext);
                }
            };
            pcInputActions.Player.MoveUp.canceled += cbContext =>
            {
                if (PlayerCanInput)
                    playerAction.StopMove(Vector2.up, cbContext);
            };

            pcInputActions.Player.MoveDown.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.down);
                }
            };
            pcInputActions.Player.MoveDown.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.down, cbContext);
                }
            };
            pcInputActions.Player.MoveDown.canceled += cbContext =>
            {
                if (PlayerCanInput)
                    playerAction.StopMove(Vector2.down, cbContext);
            };

            pcInputActions.Player.MoveLeft.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.left);
                }
            };
            pcInputActions.Player.MoveLeft.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.left, cbContext);
                }
            };
            pcInputActions.Player.MoveLeft.canceled += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.StopMove(Vector2.left, cbContext);
                }
            };

            pcInputActions.Player.MoveRight.started += ctx =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.right);
                }
            };
            pcInputActions.Player.MoveRight.performed += cbContext =>
            {
                if (PlayerCanInput)
                {
                    playerAction.Move(Vector2.right, cbContext);
                }
            };
            pcInputActions.Player.MoveRight.canceled += cbContext =>
            {
                if (PlayerCanInput)
                    playerAction.StopMove(Vector2.right, cbContext);
            };

            pcInputActions.Player.Bomb.performed += cbContext =>
            {
                //Debug.Log("Bomb PlayerCanInput" + PlayerCanInput);
                if (PlayerCanInput)
                    playerAction.Bomb();
            };
        }
    }

}
