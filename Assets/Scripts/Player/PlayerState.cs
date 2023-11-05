using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActionType
{
    None,
    Move=1,
    Bomb,
    PutBomb,
}
public class PlayerState
{
    public Vector2 pos;
    public int height;
    public int holdBombNum;
    public Vector2[] bombPos;
}


