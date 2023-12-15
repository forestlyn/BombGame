using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedoManager : MonoBehaviour
{
    private RedoCommandList redo;

    private static RedoManager instance;
    public static RedoManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        redo = new RedoCommandList();
    }

    public void AddCommand(Command command)
    {
        redo.AppendRedoCommand(command, true);
    }

    /// <summary>
    /// ÖØ×ö
    /// </summary>
    public void Redo()
    {
        Command command = redo.GetUndoCommand(out bool flag);
        if (!flag) return;

        command.Execute();

        redo.RedoHelp(command);
    }

    /// <summary>
    /// ³·Ïú
    /// </summary>
    public void Undo()
    {
        Command command = redo.GetRedoCommand(out bool flag);
        if (!flag) return;

        undo(command);

        redo.UndoHelp(command);
    }

    private void undo(Command cmd)
    {
        foreach (Command command in cmd.Next)
        {
            undo(command);
        }
        cmd.Undo();
    }

}
