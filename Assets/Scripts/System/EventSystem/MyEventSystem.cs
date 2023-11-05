using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void Mydelegate(Vector2 pos);
public class MyEventSystem : MonoBehaviour
{
    private static MyEventSystem instance;
    public static MyEventSystem Instance { get { return instance; } }

    public event Mydelegate BombEvent;
    private void Awake()
    {
        instance = this;
    }


    public void InvokeBomb(Vector2 pos)
    {
        BombEvent?.Invoke(pos);
    }


}
