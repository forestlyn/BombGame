using MyInputSystem;
using UnityEngine;

public abstract class MapObject : MonoBehaviour, IMapObjectPosition, IMapObjectEvent
{
    public Vector2 WorldPos => transform.position;

    public Vector2 ArrayPos => MapManager.CalMapPos(transform.position);

    public int id;
    public int objectId;
    public bool open;
    public abstract void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command);

    protected void MoveTo(Vector2 prePos,Vector2 afterPos)
    {
        MapManager.Instance.MoveMapObj(prePos, afterPos, this);
    }
}