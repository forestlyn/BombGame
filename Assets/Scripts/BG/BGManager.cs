using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGManager : MonoBehaviour
{

    public GameObject StartOrChooseBG;
    public GameObject PlayBG;

    private void Start()
    {
        TransitionManager.Instance.OnAfterLoadSceneEvent += ChangeBG;
    }

    private void OnDestroy()
    {
        TransitionManager.Instance.OnAfterLoadSceneEvent -= ChangeBG;
    }

    private void ChangeBG(object sender, EventArgs eventArgs)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("SceneName: " + sceneName);
        if (sceneName == "Start" || sceneName == "Choose")
        {
            StartOrChooseBG.SetActive(true);
            PlayBG.SetActive(false);
        }
        else if (sceneName == "Play")
        {
            StartOrChooseBG.SetActive(false);
            PlayBG.SetActive(true);
        }
    }
}
