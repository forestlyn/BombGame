using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public void Explosion()
    {
        Debug.Log("explosion!");
        MyEventSystem.Instance.InvokeBomb(transform.position);
    }
}
