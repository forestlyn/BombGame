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

        internal void Undo(MyInputCallbackContext cbContext)
        {
            //Debug.Log(cbContext.interaction is HoldInteraction);
            if (cbContext.interaction is MyInputInteraction.Hold)
            {
                StopUndo();
                undo = MyCoroutines.StartCoroutine(StartContinuousRedo());
            }
            if (cbContext.interaction is MyInputInteraction.Tap)
            {
                StopUndo();
            }
        }


        internal void StopUndo(MyInputCallbackContext cbContext)
        {
            if (cbContext.interaction is MyInputInteraction.Hold)
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

