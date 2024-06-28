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
    /// �����¼�
    /// </summary>
    /// <param name="mapEvent">�¼�����</param>
    /// <param name="happenPos">�¼������ص�</param>
    /// <param name="command"></param>
    public void HandleEvent(MapEventType mapEvent,Vector2 happenPos,Command command);
}