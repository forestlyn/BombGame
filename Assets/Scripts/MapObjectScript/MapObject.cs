using MyInputSystem;
using UnityEngine;

public abstract class MapObject : MonoBehaviour, IMapObjectPosition, IMapObjectEvent
{
    public Vector2 WorldPos => transform.position;

    public Vector2 ArrayPos => MapManager.CalMapPos(transform.position);

    public abstract void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command);
}