using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOrChooseBG : MonoBehaviour
{
    public float Horizontal = 2;
    public float Vertical = 1.5f;
    public float X = 30;
    public float Y = 40;

    public float RotateAngle = 30;
    public float Speed = 1;

    public GameObject bombBg;


    private bool isRunning = false;
    [SerializeField]
    private Vector3 Dir;
    private float ResetTime;
    private void Awake()
    {
        Debug.Log("X: " + X + " Y: " + Y);
        for (float i = -X / 2; i < X / 2; i += Horizontal)
        {
            for (float j = -Y / 2; j < Y / 2; j += Vertical)
            {
                GameObject bomb = Instantiate(bombBg, new Vector3(i, j, 0), transform.rotation);
                bomb.transform.SetParent(transform);
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, RotateAngle);
        Dir = new Vector3(Mathf.Sin(RotateAngle * Mathf.Deg2Rad), -Mathf.Cos(RotateAngle * Mathf.Deg2Rad), 0);
        ResetTime = 2 * Vertical / Speed;
    }

    private void Update()
    {
        if (isRunning)
            Run();
    }

    private float timer = 0;
    private void Run()
    {
        transform.position += Speed * Dir * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= ResetTime)
        {
            timer = 0;
            OnReset();
        }
    }

    private void OnReset()
    {
        transform.position = Vector3.zero;
    }

    private void OnEnable()
    {
        OnReset();
        isRunning = true;
    }

    private void OnDisable()
    {
        isRunning = false;
    }


}