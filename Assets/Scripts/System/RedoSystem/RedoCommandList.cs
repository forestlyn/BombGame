
using MyInputSystem;
using System;
using System.Collections.Generic;

public class RedoCommandList
{
    /// <summary>
    /// ���������У����������������
    /// </summary>
    private List<Command> undoList;
    /// <summary>
    /// �ɳ������У����Ѿ����˵ķ�������
    /// </summary>
    private List<Command> redoList;

    public RedoCommandList()
    {
        undoList = new List<Command>();
        redoList = new List<Command>();
    }

    /// <summary>
    /// �Ѿ�ִ���¼���������
    /// </summary>
    /// <param name="command"></param>
    /// <param name="flag">true ��ִ���¼������������¼�</param>
    public virtual void AppendRedoCommand(Command command, bool flag)
    {
        redoList.Add(command);
        if (flag)
        {
            undoList.Clear();
        }
    }
    /// <summary>
    /// �������¼���������
    /// </summary>
    /// <param name="command"></param>
    public void AppendUndoCommand(Command command)
    {
        undoList.Add(command);
    }
    /// <summary>
    /// ����ѳ������¼������Ƴ�
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
    /// ����Ѿ����˵��¼������Ƴ�
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