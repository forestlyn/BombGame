using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RedoState
{

}
public enum RedoObjectType
{
    Player=1,
}
public class RedoManager : MonoBehaviour,ICanRedo
{
    private BaseRedoObject<RedoState> redo;

    private int registerRedoObjects = 0;
    private bool isRegisterObject(RedoObjectType type)
    {
        return ((int)type & registerRedoObjects) != 0;
    }

    private static RedoManager instance;
    public static RedoManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        redo = new BaseRedoObject<RedoState>();
    }

    public void RegisterRedo(RedoObjectType type)
    {
        registerRedoObjects |= (int)type;
    }
    public void AddActionObjectType(RedoObjectType type)
    {
        ObjectAction<RedoState> action;
        action.type = (int)type;
        redo.AppendRedoAction(action, true);
    }

    public void Redo()
    {
        bool flag;
        ObjectAction<RedoState> action = redo.GetUndoAction(out flag);
        if (!flag) return;
        if (!isRegisterObject((RedoObjectType)action.type)) return;
        switch (action.type)
        {
            case (int)RedoObjectType.Player:
                Player.Instance.Redo();
                break;
        }
        redo.RedoHelp(action);
    }

    public void Undo()
    {
        bool flag;
        ObjectAction<RedoState> action = redo.GetRedoAction(out flag);
        if (!flag) return;
        if (!isRegisterObject((RedoObjectType)action.type)) return;
        switch (action.type)
        {
            case (int)RedoObjectType.Player:
                Player.Instance.Undo();
                break;
        }
        redo.UndoHelp(action);
    }
}
