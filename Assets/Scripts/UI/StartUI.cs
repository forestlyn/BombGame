using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public Button fullScreenButton;
    void Start()
    {
        startButton.onClick.AddListener(LoadChooseScene);
        exitButton.onClick.AddListener(ExitGame);
#if UNITY_ANDROID
        fullScreenButton.gameObject.SetActive(false);
#else
        fullScreenButton.onClick.AddListener(ChangeFullScreen);
#endif
    }

    private void LoadChooseScene()
    {
        TransitionManager.Instance.Transition("Start", "Choose");
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ChangeFullScreen()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(1920, 1080, false);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
    }
}
