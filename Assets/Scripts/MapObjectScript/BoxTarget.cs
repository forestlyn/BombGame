using MyInputSystem;
using UnityEngine;

public class BoxTarget : MapObject
{
    public bool Open
    {
        get => open;
        set
        {
            if (open != value)
            {
                open = value;
                MyEventSystem.Instance.InvokeBoxTargetStateChange(value, objectId);
            }
        }
    }
    private void OnEnable()
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
        //Debug.Log("check");
        var list = MapManager.Instance.MapObjs(arrayPos);
        foreach (var item in list)
        {
            switch (item.type)
            {
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