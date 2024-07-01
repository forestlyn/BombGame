using MyInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RedoManager : MonoBehaviour
{
    private RedoCommandList redoCommandList;

    private static RedoManager instance;
    public static RedoManager Instance { get { return instance; } }

    public static bool isUndo = false;
    private void Awake()
    {
        instance = this;
        redoCommandList = new RedoCommandList();
    }

    public void ClearCommandLists()
    {
        redoCommandList.ClearLists();
    }

    public void AddCommand(Command command)
    {
        redoCommandList.AppendRedoCommand(command, true);
    }

    /// <summary>
    /// ÖØ×ö
    /// </summary>
    public void Redo()
    {
        Command command = redoCommandList.GetUndoCommand(out bool flag);
        if (!flag) return;

        redo(command);

        redoCommandList.RedoHelp(command);
    }

    /// <summary>
    /// ³·Ïú
    /// </summary>
    public void Undo()
    {
        Command command = redoCommandList.GetRedoCommand(out bool flag);
        if (!flag) return;
        //print(command);
        //Debug.Log("/////");
        undo(command);

        redoCommandList.UndoHelp(command);
    }

    private void print(Command cmd) {
        Debug.Log(cmd);
        foreach (var c in cmd.Next)
        {
            print(c);
        }
    }

    private void undo(Command cmd,bool isFirstCMD = true)
    {
        isUndo = true;
        for(int i = cmd.Next.Count - 1; i >= 0; i--)
        {
            undo(cmd.Next[i], false);
        }
        //Debug.Log(cmd);
        cmd.Undo();
        isUndo = isFirstCMD ? false : true;
    }
    private void redo(Command cmd)
    {
        //Debug.Log(cmd);
        cmd.Execute();
        foreach (Command command in cmd.Next)
        {
            redo(command);
        }
    }
}
