using System.Collections.Generic;

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
        //���������������ǳ������ɴ������
        if (!MyInputSystem.InputManager.PlayerCanInput || RedoManager.isUndo) return false;
        for (int i = 0; i < objStates.Count; i++)
        {
            if (objStates[i].mapObject.open != true) {
                return false;
            }
        }
        return true;
    }
}