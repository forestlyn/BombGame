using MyTool.Music;
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
        MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
        TransitionManager.Instance.Transition("Start", "Choose");
    }

    private void ExitGame()
    {
        MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ChangeFullScreen()
    {
        MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
        if (Screen.fullScreen)
        {
            Screen.SetResolution(1920, 1080, false);
        }
        else
        {
            SetFullScreen();
        }
    }

    public static void SetFullScreen()
    {
        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;
        float m_width = height * 16 / 9;
        float m_height = width * 9 / 16;
        Debug.Log("width:" + width + " height:" + height + " m_width:" + m_width + " m_height:" + m_height);
        if (m_width > width)
        {
            Screen.SetResolution((int)width, (int)m_height, true);
        }
        else
        {
            Screen.SetResolution((int)m_width, (int)height, true);
        }
    }
}
