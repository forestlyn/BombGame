
using MyInputSystem;
using UnityEngine;

public class MapObjIntoWater : Command
{
    MapObject mapObj;
    Vector2 worldPos;
    Vector2 arrayPos;
    BaseMapObjectState state;
    public MapObjIntoWater(MapObject mapObj) : base(mapObj)
    {
        this.mapObj = mapObj;
        worldPos = mapObj.WorldPos;
        arrayPos = mapObj.ArrayPos;
    }
    public override void Execute()
    {
        state = mapObj.MyDestory();
        mapObj.transform.position = MapObject.hiddenPos;
    }

    public override void Undo()
    {
        MapManager.Instance.MapObjs(arrayPos).Add(state);
        mapObj.transform.position = worldPos;
        //Debug.Log(mapObj.type +""+ worldPos);
        if (mapObj.type == MapObjectType.Bomb)
        {
            Player.Instance.AddBomb(mapObj as Bomb);
        }
    }
}