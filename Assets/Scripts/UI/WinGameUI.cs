using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGameUI : MonoBehaviour
{
    public Button returnMenuBtn;
    public Button nextLevelBtn;
    public Text tipText;
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

    private void OnEnable()
    {
        if (GameManager.Instance.HasNextLevel())
        {
            nextLevelBtn.gameObject.SetActive(true);
            tipText.gameObject.SetActive(true);
        }
        else
        {
            nextLevelBtn.gameObject.SetActive(false);
            tipText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.HasNextLevel())
            return;
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            GameManager.Instance.NextLevel();
        }
    }
}
