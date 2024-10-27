using MyTools.MyCoroutines;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using System.Collections;

namespace MyInputSystem
{
    public class GameAction
    {
        public void Restart()
        {
            var input = new PlayerInput(PlayerInputType.Restart);
            PlayerInputManager.Instance.Add(input);
        }
        public void Redo()
        {
            var input = new PlayerInput(PlayerInputType.Redo);
            PlayerInputManager.Instance.Add(input);
        }

        public void Undo()
        {
            var input = new PlayerInput(PlayerInputType.Undo);
            PlayerInputManager.Instance.Add(input);
        }

        MyCoroutine undo;

        public void ShowGrid()
        {
            GameManager.Instance.ShowGrid();
        }

        internal void Undo(InputAction.CallbackContext cbContext)
        {
            //Debug.Log(cbContext.interaction is HoldInteraction);
            if (cbContext.interaction is HoldInteraction)
            {
                StopUndo();
                undo = MyCoroutines.StartCoroutine(StartContinuousRedo());
            }
            if (cbContext.interaction is TapInteraction)
            {
                StopUndo();
            }
        }


        internal void StopUndo(InputAction.CallbackContext cbContext)
        {
            if (cbContext.interaction is HoldInteraction)
            {
                StopUndo();
            }
        }

        private void StopUndo()
        {
            MyCoroutines.StopCoroutine(undo);
        }

        private IEnumerator StartContinuousRedo()
        {
            while (true)
            {
                Undo();
                yield return new YieldWaitForSeconds(0.15f);
            }
        }
    }
}

