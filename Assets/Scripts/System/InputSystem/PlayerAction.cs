using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;

namespace MyInputSystem
{
    public class PlayerAction
    {
        internal void Move(Vector2 dir, InputAction.CallbackContext cbContext)
        {
            //Debug.Log("move" + cbContext.ReadValue<float>() + " " + cbContext.interaction);
            if (cbContext.interaction is HoldInteraction)
            {
                //Debug.Log("长按");
                Player.Instance.Move(dir, true);
            }
            if (cbContext.interaction is TapInteraction)
            {
                //Debug.Log("点按");
                Player.Instance.Move(dir, false);
            }
        }
        internal void StopMove(Vector2 dir, InputAction.CallbackContext cbContext)
        {
            if (cbContext.interaction is HoldInteraction)
            {
                Player.Instance.StopMove(dir);
            }
        }

        internal void Bomb()
        {
            Player.Instance.Bomb();
        }
    }
}

