using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanRedo
{
    /// <summary>
    /// ����
    /// </summary>
    public void Redo();
    /// <summary>
    /// ����
    /// </summary>
    public void Undo();
}