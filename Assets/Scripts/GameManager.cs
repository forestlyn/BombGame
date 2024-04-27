using MyTools.MyCoroutines;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string path;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public string StartSceneName;

    private string loadMapFile;
    private List<string> mapFiles = new List<string>();

    public List<string> MapFiles
    {
        get => mapFiles;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        path = Application.streamingAssetsPath;
        GetAllLevels();

        TransitionManager.Instance.Transition("", StartSceneName);
    }
    private void GetAllLevels()
    {
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith("json"))
            {
                mapFiles.Add(file);
            }
        }
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

    public void WinGame()
    {
        MapManager.Instance.WinGame();
    }

    private void OnAfterLoadScene()
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            MapManager.Instance.LoadMapFromFile(loadMapFile);
        }
    }
}
