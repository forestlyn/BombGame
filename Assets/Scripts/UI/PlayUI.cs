using MyTool.Music;
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
    public Button backToChooseButton;
    public Button backToMenuButton;
    public Button gridOnOffButton;

    public GameObject settingPanel;
    private void Start()
    {
        settingButton.onClick.AddListener(() =>
        {
            settingPanel.SetActive(true);
            UpdateUI();
            MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
        });
        backToGameButton.onClick.AddListener(() =>
        {
            settingPanel.SetActive(false);
            MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
        });
        backToMenuButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
            TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Start");
        });
        backToChooseButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
            TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Choose");
        });
        gridOnOffButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.PlayEffect(MusicEnum.ButtonClick);
            GameManager.Instance.ShowGrid();
            bool gridOn = GameManager.Instance.GridOn;
            gridOnOffButton.GetComponentInChildren<Text>().text = gridOn ? "网格：开" : "网格：关";
        });
        UpdateUI();
    }


    private void UpdateUI()
    {
        bool gridOn = GameManager.Instance.GridOn;
        gridOnOffButton.GetComponentInChildren<Text>().text = gridOn ? "网格：开" : "网格：关";
    }
    private void Update()
    {
        if (!GameManager.Instance.isGameWin && Input.GetKeyDown(KeyCode.Escape))
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
        }
    }
}
