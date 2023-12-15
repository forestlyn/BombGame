using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapState
{
    public int length, width;
    public BaseMapObjectState[,] Map;
}

public class BaseMapObjectState
{
    public MapObjectType type;
    public int height;
    public MapObject mapObject;
    public BaseMapObjectState(MapObjectType type,int height)
    {
        this.type = type;
        this.height = height;
    }
}

public class DoorState : BaseMapObjectState
{
    public DoorState(MapObjectType type, int height) : base(type, height)
    {
    }
}

public class SpecialDoorState : BaseMapObjectState
{
    public SpecialDoorState(MapObjectType type, int height) : base(type, height)
    {
    }
}
