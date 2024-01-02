using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Button startButton;
    void Start()
    {
        startButton.onClick.AddListener(LoadChooseScene);
    }

    private void LoadChooseScene()
    {
        TransitionManager.Instance.Transition("Start", "Choose");
    }
    void Update()
    {
        
    }
}
