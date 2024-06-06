using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public Button settingButton;
    public Button backToGameButton;
    public Button backToMenuButton;

    public GameObject settingPanel;
    private void Start()
    {
        settingButton.onClick.AddListener(() => settingPanel.SetActive(true));
        backToGameButton.onClick.AddListener(() => settingPanel.SetActive(false));
        backToMenuButton.onClick.AddListener(() =>
        {
            TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Start");
        });
    }

}
