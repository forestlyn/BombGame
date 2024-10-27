using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SpeedBecomeZero();
public interface IMove
{
    //ÒÆ¶¯µÄ¾àÀë
    public float MoveDistance { get; set; }

    //public Vector2 Dir { get; set; }

    public Vector2 Target { get; set; }

    //public float Deceleration { get; set; }

    //public Transform MoveTransform { get; set; }

    public bool IsMoving { get;}

    public abstract void StopMove();
    public abstract void Move(float interval);

    public event SpeedBecomeZero OnSpeedBecomeZero;

}
