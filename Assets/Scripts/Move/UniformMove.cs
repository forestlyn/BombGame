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

        // ���㵱ǰ֡Ӧ���ƶ��ľ���
        float distanceToMove = CurrentSpeed * interval;

        // ����ƶ�������ڵ���Ŀ��λ���뵱ǰλ��֮��ľ��룬��ֱ�ӽ������ƶ���Ŀ��λ��
        if (distanceToMove >= direction.magnitude)
        {
            transform.position = target;
            CurrentSpeed = 0;
            isMoving = false;
            OnSpeedBecomeZero.Invoke();
        }
        else
        {
            // ���򣬰����ƶ�����;����ƶ�����
            transform.position = pos + direction.normalized * distanceToMove;
        }
    }

    public void StopMove()
    {
        
    }
}