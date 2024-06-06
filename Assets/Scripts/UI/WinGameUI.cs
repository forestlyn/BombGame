using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGameUI : MonoBehaviour
{
    public Button returnMenuBtn;
    public Button nextLevelBtn;

    public void Start()
    {
        returnMenuBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Start");
        });
        nextLevelBtn.onClick.AddListener(() =>
        {
            MapManager.Instance.NextLevel();
        });
    }
}
