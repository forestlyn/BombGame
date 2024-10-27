using System.Diagnostics;

namespace MyInputSystem
{
    public class GameReStartCommand : Command
    {
        MapState mapState;
        MapManager mapManager;
        public GameReStartCommand(MapManager mapManager):base(null)
        {
            this.mapManager = mapManager;
        }
        public override void Execute()
        {
            mapState = mapManager.GetMapState();
            mapManager.ReStart();
        }

        public override void Undo()
        {
            mapManager.SetMapState(mapState);
        }
    }
}