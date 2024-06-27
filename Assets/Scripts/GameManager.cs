using MyTools.MyCoroutines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string path;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public string StartSceneName;

    private string loadMapFile;
    private List<MapFiles> mapFiles = new List<MapFiles>();

    /// <summary>
    /// 当前关卡
    /// </summary>
    private int currentLevel = 0;
    /// <summary>
    /// 当前大关卡
    /// </summary>
    private int currentMapLevel = 0;


    public List<MapFiles> MapFiles
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
        try
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                Debug.Log(dir);
                string[] files = Directory.GetFiles(dir);
                List<string> jsonFiles = new List<string>();
                foreach (var file in files)
                {
                    if (file.EndsWith("json"))
                    {
                        jsonFiles.Add(file);
                    }
                }
                var dirsplit = Path.GetFileName(dir).Split(' ');
                //Debug.Log($"{dirsplit[0]}");
                var dirname = dir;
                if (dirsplit.Length == 2)
                {
                    dirname = dirsplit[1];
                }
                //Debug.Log($"{dirname}:{dirsplit[0]}");
                mapFiles.Add(new MapFiles(dirsplit[0], dirname, jsonFiles));
            }
        }
        catch(Exception e) {
            Debug.Log("GetAllLevels err" + e.ToString());
        }
    }
    private void OnEnable()
    {
        TransitionManager.Instance.OnAfterLoadSceneEvent += OnAfterLoadScene;
        TransitionManager.Instance.OnStartLoadSceneEvent += OnStartLoadScene;
    }



    private void OnDisable()
    {
        TransitionManager.Instance.OnAfterLoadSceneEvent -= OnAfterLoadScene;
        TransitionManager.Instance.OnStartLoadSceneEvent -= OnStartLoadScene;
    }

    void Update()
    {
        MyCoroutines.Update(Time.deltaTime);
    }

    public void LoadMap(string file)
    {
        currentLevel = mapFiles[currentMapLevel].LevelFile.FindIndex(x => string.Equals(file, x.levelDir));
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
    private void OnStartLoadScene()
    {
        Player.Instance.transform.position = MapObject.hiddenPos;
    }
    public void WinGame()
    {
        MapManager.Instance.WinGame();
    }

    public bool HasNextLevel()
    {
        if (mapFiles[currentMapLevel].LevelFile.Count > currentLevel + 1)
        {
            return true;
        }
        return false;
    }

    public void SetMapLevel(int maplevel)
    {
        currentMapLevel = maplevel;
    }

    public void NextLevel()
    {
        currentLevel++;
        Debug.Log($"{currentLevel}:{mapFiles[currentMapLevel].LevelFile[currentLevel].levelDir}");
        LoadMap(mapFiles[currentMapLevel].LevelFile[currentLevel].levelDir);
    }
}
