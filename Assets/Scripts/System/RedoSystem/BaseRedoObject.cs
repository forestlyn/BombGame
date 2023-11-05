using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基本的重做物体
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRedoObject<T>
{
    /// <summary>
    /// 可重做队列，即撤销后放入其中
    /// </summary>
    private List<ObjectAction<T>> undoList;
    /// <summary>
    /// 可撤销队列，即已经做了的放入其中
    /// </summary>
    private List<ObjectAction<T>> redoList;

    public BaseRedoObject()
    {
        undoList = new List<ObjectAction<T>>();
        redoList = new List<ObjectAction<T>>();
    }

    /// <summary>
    /// 已经执行事件放入其中
    /// </summary>
    /// <param name="action"></param>
    /// <param name="flag">true 新执行事件，而非重做事件</param>
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
    /// 撤销的事件放入其中
    /// </summary>
    /// <param name="action"></param>
    public void AppendUndoAction(ObjectAction<T> action)
    {
        undoList.Add(action);
    }
    /// <summary>
    /// 获得已撤销的事件
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
    /// 获得已经做了的事件
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
    /// 帮助将重做的事件添加到UndoList
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
    /// 帮助将撤销的事件添加到RedoList
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
