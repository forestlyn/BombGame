using MyInputSystem;
using UnityEngine;

public class Bomb : MapObject
{
    public void Explosion(Command command)
    {
        Debug.Log("explosion!");
        MyEventSystem.Instance.InvokeEvent(InvokeEventType.Four, MapEventType.Bomb, WorldPos, command);
    }


    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
    }
}
