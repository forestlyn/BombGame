using MyInputSystem;
using UnityEngine;

public class PressBoard : MapObject
{
    public bool Open
    {
        get => open;
        set
        {
            if (open != value)
            {
                MyEventSystem.Instance.InvokePressBoardStateChange(value, id);
            }
            open = value;
        }
    }
    private void Start()
    {
        Check(ArrayPos);
    }
    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.Leave:
            case MapEventType.Arrive:
                Check(ArrayPos);
                break;
        }
    }

    private void Check(Vector2 arrayPos)
    {
        Debug.Log("check");
        var list = MapManager.Instance.MapObj(arrayPos);
        foreach (var item in list)
        {
            switch (item.type)
            {
                case MapObjectType.Player:
                case MapObjectType.Box:
                    Open = true;
                    return;
                default:
                    break;
            }
        }
        Open = false;
    }
}