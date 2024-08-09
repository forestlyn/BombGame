using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformVariableMove : MonoBehaviour, IMove
{
    private Vector2 target;
    private float moveDistance;
    [Min(0.01f)]
    public float deceleration;
    public Transform moveTranform;

    [SerializeField]
    private bool isMoving = false;

    public float CurrentSpeed;

    public Vector2 Target
    {
        get => target;
        set
        {
            target = value;
            isMoving = true;
            CurrentSpeed = CalculateSpeed();
        }
    }
    public float MoveDistance { get => moveDistance; set => moveDistance = value; }

    public bool IsMoving => isMoving;

    public event SpeedBecomeZero OnSpeedBecomeZero;

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

        float nextSpeed = Mathf.Max(CurrentSpeed - deceleration * interval, 0);

        // ���㵱ǰ֡Ӧ���ƶ��ľ���
        float distanceToMove = (CurrentSpeed + nextSpeed) / 2.0f * interval;


        // ����ƶ�������ڵ���Ŀ��λ���뵱ǰλ��֮��ľ��룬���߾��빻������ֱ�ӽ������ƶ���Ŀ��λ��
        if (distanceToMove >= direction.magnitude || distanceToMove <= 0.0001f)
        {
            transform.position = target;
            CurrentSpeed = 0;
            isMoving = false;
            OnSpeedBecomeZero?.Invoke();
            MoveDistance = 0;
        }
        else
        {
            // ���򣬰����ƶ�����;����ƶ�����
            transform.position = pos + direction.normalized * distanceToMove;
            CurrentSpeed = nextSpeed; 
        }
    }

    private float CalculateSpeed()
    {
        float realMoveDis = Vector2.Distance(moveTranform.position, target);
        float dis = realMoveDis < MoveDistance ? MoveDistance : realMoveDis;
        return Mathf.Sqrt(2 * deceleration * dis);
    }

    public void StopMove()
    {
    }
}
