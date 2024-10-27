
using MyInputSystem;
using System;
using System.Collections.Generic;

public class RedoCommandList
{
    /// <summary>
    /// 可重做队列，即撤销后放入其中
    /// </summary>
    private List<Command> undoList;
    /// <summary>
    /// 可撤销队列，即已经做了的放入其中
    /// </summary>
    private List<Command> redoList;

    public RedoCommandList()
    {
        undoList = new List<Command>();
        redoList = new List<Command>();
    }

    /// <summary>
    /// 已经执行事件放入其中
    /// </summary>
    /// <param name="command"></param>
    /// <param name="flag">true 新执行事件，而非重做事件</param>
    public virtual void AppendRedoCommand(Command command, bool flag)
    {
        redoList.Add(command);
        if (flag)
        {
            undoList.Clear();
        }
    }
    /// <summary>
    /// 撤销的事件放入其中
    /// </summary>
    /// <param name="command"></param>
    public void AppendUndoCommand(Command command)
    {
        undoList.Add(command);
    }
    /// <summary>
    /// 获得已撤销的事件，并移除
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public Command GetUndoCommand(out bool flag)
    {
        Command t = null;
        if (undoList.Count != 0)
        {
            flag = true;
            t = undoList[
                undoList.Count - 1];
            undoList.RemoveAt(undoList.Count - 1);
        }
        else
            flag = false;
        return t;


    }
    /// <summary>
    /// 获得已经做了的事件，并移除
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public Command GetRedoCommand(out bool flag)
    {
        Command t = null;
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

    internal void RedoHelp(Command command)
    {
        AppendRedoCommand(command, false);
    }

    internal void UndoHelp(Command command)
    {
        AppendUndoCommand(command);
    }
}