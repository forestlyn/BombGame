using MyInputSystem;
using MyTool.Music;
using UnityEngine;

public abstract class MapObject : MonoBehaviour, IMapObjectPosition, IMapObjectEvent
{
    public static Vector2 hiddenPos = new Vector2(100, 100); 

    public Vector2 WorldPos => transform.position;

    public Vector2 ArrayPos => MapManager.CalMapPos(transform.position);

    public int objectId;

    public bool open;

    public MapObjectType type;
    public abstract void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command);

    protected void MoveTo(Vector2 prePos,Vector2 afterPos)
    {
        MapManager.Instance.MoveMapObj(prePos, afterPos, this);
    }
    public virtual BaseMapObjectState MyDestory()
    {
        return MapManager.Instance.RemoveMapObj(this);
    }

    public virtual void Initialize()
    {

    }
}