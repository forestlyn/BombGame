using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    void Start()
    {
        startButton.onClick.AddListener(LoadChooseScene);
        exitButton.onClick.AddListener(ExitGame);
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
}
