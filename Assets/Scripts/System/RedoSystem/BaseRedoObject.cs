using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������������
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRedoObject<T>
{
    /// <summary>
    /// ���������У����������������
    /// </summary>
    private List<ObjectAction<T>> undoList;
    /// <summary>
    /// �ɳ������У����Ѿ����˵ķ�������
    /// </summary>
    private List<ObjectAction<T>> redoList;

    public BaseRedoObject()
    {
        undoList = new List<ObjectAction<T>>();
        redoList = new List<ObjectAction<T>>();
    }

    /// <summary>
    /// �Ѿ�ִ���¼���������
    /// </summary>
    /// <param name="action"></param>
    /// <param name="flag">true ��ִ���¼������������¼�</param>
    public virtual void AppendRedoAction(ObjectAction<T> action, bool flag = false)
    {
        //Debug.Log(typeof(T) + " " + action.type + " " + flag);
        redoList.Add(action);
        if (flag)
        { 
            undoList.Clear(); 
        }
    }
    /// <summary>
    /// �������¼���������
    /// </summary>
    /// <param name="action"></param>
    public void AppendUndoAction(ObjectAction<T> action)
    {
        undoList.Add(action);
    }
    /// <summary>
    /// ����ѳ������¼�
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public ObjectAction<T> GetUndoAction(out bool flag)
    {
        ObjectAction<T> t;
        t.type = 0;
        t.endState = t.startState = default;
        if (undoList.Count != 0)
        {
            flag = true;
            t = undoList[undoList.Count - 1];
            undoList.RemoveAt(undoList.Count - 1);
        }
        else
            flag = false;
        return t;
    }
    /// <summary>
    /// ����Ѿ����˵��¼�
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public ObjectAction<T> GetRedoAction(out bool flag)
    {
        ObjectAction<T> t;
        t.type = 0;
        t.endState = t.startState = default;
        if (redoList.Count != 0)
        {
            flag = true;
            t = redoList[redoList.Count - 1];
            redoList.RemoveAt(redoList.Count - 1);
        }
        else
            flag = false;
        return t;
    }
    /// <summary>
    /// �������������¼���ӵ�UndoList
    /// </summary>
    /// <param name="action"></param>
    public void RedoHelp(ObjectAction<T> action)
    {
        ObjectAction<T> t = action;
        t.endState = action.startState;
        t.startState = action.endState;
        this.AppendRedoAction(t);
    }
    /// <summary>
    /// �������������¼���ӵ�RedoList
    /// </summary>
    /// <param name="action"></param>
    public void UndoHelp(ObjectAction<T> action)
    {
        ObjectAction<T> t = action;
        t.endState = action.startState;
        t.startState = action.endState;
        this.AppendUndoAction(t);
    }
}
