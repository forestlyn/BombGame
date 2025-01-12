using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBG : MonoBehaviour
{
    private bool isRunning = false;

    private List<PlayCubeBG> playCubeBGs = new List<PlayCubeBG>();

    public GameObject PlayCubeBgPrefab;

    public int PlayCubeBgCount = 10;
    void Awake()
    {
        for (int i = 0; i < PlayCubeBgCount; i++)
        {
            GameObject go = Instantiate(PlayCubeBgPrefab, transform);
            PlayCubeBG playCubeBG = go.GetComponent<PlayCubeBG>();
            playCubeBGs.Add(playCubeBG);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
            return;

        foreach (var item in playCubeBGs)
        {
            item.MyUpdate(Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        isRunning = true;
    }

    private void OnDisable()
    {
        isRunning = false;
    }
}
