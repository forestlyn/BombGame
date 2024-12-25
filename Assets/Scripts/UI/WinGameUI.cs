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
            TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Start");
        });
        nextLevelBtn.onClick.AddListener(() =>
        {
            if (GameManager.Instance.HasNextLevel())
                GameManager.Instance.NextLevel();
            else
            {
                GameManager.Instance.SetMapLevel(GameManager.Instance.currentMapLevel + 1);
                TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Choose");
            }
        });
    }

    private void OnEnable()
    {
        if (GameManager.Instance.HasNextLevel())
        {
            nextLevelBtn.gameObject.SetActive(true);
            tipText.gameObject.SetActive(true);
#if UNITY_ANDROID
            tipText.text = "";
#else
            tipText.text = "按任意键进入下一关";
#endif
        }
        else if (GameManager.Instance.HasNextBigLevel())
        {
            nextLevelBtn.gameObject.SetActive(true);
            tipText.gameObject.SetActive(true);
#if UNITY_ANDROID
            tipText.text = "";
#else
            tipText.text = "按任意键进入下一大关";
#endif
        }
        else
        {
            nextLevelBtn.gameObject.SetActive(false);
            tipText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.HasNextLevel() && !GameManager.Instance.HasNextBigLevel())
            return;
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            if (GameManager.Instance.HasNextLevel())
                GameManager.Instance.NextLevel();
            else
            {
                GameManager.Instance.SetMapLevel(GameManager.Instance.currentMapLevel + 1);
                TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Choose");
            }
        }
    }
}
