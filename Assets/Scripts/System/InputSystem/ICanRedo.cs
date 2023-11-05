using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanRedo
{
    /// <summary>
    /// ÖØ×ö
    /// </summary>
    public void Redo();
    /// <summary>
    /// ³·Ïú
    /// </summary>
    public void Undo();
}