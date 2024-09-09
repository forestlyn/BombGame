using MyInputSystem;
using System.Collections.Generic;
using UnityEngine;
public static class CheckGameWin
{
    private static List<BaseMapObjectState> objStates = new List<BaseMapObjectState>();

    //private static HashSet<int> isMovingId = new HashSet<int>();
    //public static void OnMovingStateChange(int instanceId, bool isMoving)
    //{
    //    Debug.Log(isMoving + "" + instanceId + " Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
    //    if(isMoving)
    //    {
    //        isMovingId.Add(instanceId);
    //    }
    //    else
    //    {
    //        isMovingId.Remove(instanceId);
    //    }
    //}

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
        MyLog.LogWithTime("check game win");
        //Debug.Log(isMovingId.Count);
        //玩家锁定输入或者是撤销,或者在动画状态不可触发检查
        //但当玩家被撞飞最后一格为终点时，检查返回导致无法成功检查win
        if (!InputManager.PlayerCanInput
            || RedoManager.isUndo
            || MyEventSystem.Instance.IsInvokingEvent)
        {
            //MyLog.Log("player can input:" + InputManager.PlayerCanInput +
            //    " IsInvokingEvent" + MyEventSystem.Instance.IsInvokingEvent);
            return false;
        }
        for (int i = 0; i < objStates.Count; i++)
        {
            if (objStates[i].mapObject.open != true)
            {
                return false;
            }
        }
        //Debug.Log("GameWin! Current time: " + System.DateTime.Now.ToString("HH:mm:ss.fff"));
        GameManager.Instance.isGameWin = true;
        return true;
    }
}