using MyTools.MyCoroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public string StartSceneName;

    private string loadMapFile;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        TransitionManager.Instance.Transition("", StartSceneName);
    }

    private void OnEnable()
    {
        TransitionManager.Instance.OnAfterLoadSceneEvent += OnAfterLoadScene;
    }
    private void OnDisable()
    {
        TransitionManager.Instance.OnAfterLoadSceneEvent -= OnAfterLoadScene;
    }

    void Update()
    {
        MyCoroutines.Update(Time.deltaTime);
    }

    public void LoadMap(string file)
    {
        loadMapFile = file;
        TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Play");
    }

    private void OnAfterLoadScene()
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            MapManager.Instance.LoadMapFromFile(loadMapFile);
        }
    }
}
