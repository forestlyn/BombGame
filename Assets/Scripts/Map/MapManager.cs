using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private MapState mapState;

    private static MapManager instance;
    public static MapManager Instance { get { return instance; } }

    private MapState GetMapState()
    {
        return mapState;
    }

    private void Awake()
    {
        instance = this;
        mapState = new MapState();
        mapState.map = new BaseMapObjectState[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                mapState.map[i, j] = new BaseMapObjectState(MapObjectType.Ground, 0);
            }
        }
        mapState.map[0, 0] = new BaseMapObjectState(MapObjectType.Wall, 1);

    }

    internal bool CanMove(Vector2 playerPos,int height)
    {
        if (mapState.map[(int)playerPos.x, (int)playerPos.y].height==height)
            return true;
        return false;
    }
}
