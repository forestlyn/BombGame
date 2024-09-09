using MyInputSystem;
using UnityEngine;

public class BoxTarget : MapObject
{
    public Material[] materials;
    public SpriteRenderer sr;
    public bool Open
    {
        get => open;
        set
        {
            if (open != value)
            {
                open = value;
                //MyEventSystem.Instance.InvokeBoxTargetStateChange(value, objectId);
                //sr.material = materials[open == true ? 1 : 0];
            }
        }
    }
    public override void Initialize()
    {
        Check(ArrayPos);
    }

    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.Leave:
            case MapEventType.Arrive:
            case MapEventType.BoxStop:
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
                    Box box = item.mapObject as Box;
                    if (box.kESimu.Energe != 0)
                        return;
                    Open = true;
                    return;
                default:
                    break;
            }
        }
        Open = false;
    }
}