using MyInputSystem;
using UnityEngine;

public class Flag : MapObject
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
                MyEventSystem.Instance.InvokeFlagStateChange(value, objectId);
                //Debug.Log("flag state becomes " + value);
                //sr.material = materials[open == true ? 1 : 0];
            }
            
        }
    }
    public override void HandleEvent(MapEventType mapEvent, Vector2 happenPos, Command command)
    {
        switch (mapEvent)
        {
            case MapEventType.Arrive:
                Check(ArrayPos);
                break;
            case MapEventType.Leave:
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
                case MapObjectType.Player:
                    Open = true;
                    return;
                default:
                    break;
            }
        }
        Open = false;
    }
}
