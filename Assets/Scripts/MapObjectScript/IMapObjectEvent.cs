using MyInputSystem;
using UnityEngine;

public enum MapEventType
{
    Bomb,
    PlayerMove,
}

public interface IMapObjectEvent
{
    public void HandleEvent(MapEventType mapEvent,Vector2 happenPos,Command command);
}