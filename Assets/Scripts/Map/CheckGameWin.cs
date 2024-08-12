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
        //Debug.Log(isMovingId.Count);
        //���������������ǳ���,�����ڶ���״̬���ɴ������
        //������ұ�ײ�����һ��Ϊ�յ�ʱ����鷵�ص����޷��ɹ����win
        if (!InputManager.PlayerCanInput
            || RedoManager.isUndo /*|| isMovingId.Count != 0*/) 
            return false;
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