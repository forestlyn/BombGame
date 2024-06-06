using System.Collections.Generic;
using System.Diagnostics;

public static class CheckGameWin
{
    private static List<BaseMapObjectState> objStates = new List<BaseMapObjectState>();

    public static void Clear()
    {
        objStates.Clear();
    }
    public static void Add(BaseMapObjectState objState)
    {
        objStates.Add(objState);
    }

    public static bool CheckWin()
    {
        for (int i = 0; i < objStates.Count; i++)
        {
            if (objStates[i].mapObject.open != true) {
                return false;
            }
        }
        return true;
    }
}