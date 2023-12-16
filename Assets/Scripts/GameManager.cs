using MyTools.MyCoroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        MyCoroutines.Update(Time.deltaTime);
    }
}
