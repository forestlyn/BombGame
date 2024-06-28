using MyInputSystem;
using UnityEngine;

public enum MapEventType
{
    Bomb,
    BoxCollision,
    BoxStop,
    PlayerMove,
    BoxMove,
    BombMove,
    PressBoardUp,
    PressBoardDown,
    Leave,
    Arrive
}

public interface IMapObjectEvent
{
    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="mapEvent">事件类型</param>
    /// <param name="happenPos">事件发生地点</param>
    /// <param name="command"></param>
    public void HandleEvent(MapEventType mapEvent,Vector2 happenPos,Command command);
}