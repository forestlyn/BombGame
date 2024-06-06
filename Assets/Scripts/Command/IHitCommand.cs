using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitCommand 
{
    public int Energe { get; }
    public Vector2 Dir {  get; }
}
