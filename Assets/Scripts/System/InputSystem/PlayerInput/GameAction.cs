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
    }
}

