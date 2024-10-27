using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UniformMove : MonoBehaviour, IMove
{
    private Vector2 target;
    private float moveDistance;
    //public float deceleration;
    public Transform moveTranform;

    [SerializeField]
    private bool isMoving = false;

    [SerializeField]
    public float moveSpeed;

    public float CurrentSpeed;

    public event SpeedBecomeZero OnSpeedBecomeZero;

    public Vector2 Target
    {
        get => target;
        set
        {
            target = value;
            isMoving = true;
            CurrentSpeed = moveSpeed;
        }
    }

    public bool IsMoving { get => isMoving; }
    public float MoveDistance { get => moveDistance; set => moveDistance = value; }

    public void Update()
    {
        if (isMoving)
        {
            Move(Time.deltaTime);
        }
    }

    public void Move(float interval)
    {
        Vector2 pos = transform.position;
        Vector2 direction = target - pos;

        // 计算当前帧应该移动的距离
        float distanceToMove = CurrentSpeed * interval;

        // 如果移动距离大于等于目标位置与当前位置之间的距离，则直接将物体移动到目标位置
        if (distanceToMove >= direction.magnitude)
        {
            transform.position = target;
            CurrentSpeed = 0;
            isMoving = false;
            OnSpeedBecomeZero.Invoke();
        }
        else
        {
            // 否则，按照移动方向和距离移动物体
            transform.position = pos + direction.normalized * distanceToMove;
        }
    }

    public void StopMove()
    {
        
    }
}