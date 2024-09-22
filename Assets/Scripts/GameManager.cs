using MyInputSystem;
using MyTools.MyCoroutines;
using System;
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
    private string currentMapName;
    private List<MapFiles> allMapFiles = new List<MapFiles>();

    /// <summary>
    /// 当前关卡
    /// </summary>
    public int currentLevel { get; private set; }
    /// <summary>
    /// 当前大关卡
    /// </summary>
    public int currentMapLevel {  get; private set; }

    public bool isAnimMoving = false;

    public List<MapFiles> AllMapFiles
    {
        get => allMapFiles;
    }
    public bool isGameWin;

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
                //Debug.Log(dir);
                string[] files = Directory.GetFiles(dir);
                List<string> jsonFiles = new List<string>();
                foreach (var file in files)
                {
                    if (file.EndsWith("json"))
                    {
                        jsonFiles.Add(file);
                    }
                }
                var dirsplit = Path.GetFileName(dir).Split(' ', 2);
                //Debug.Log($"{dirsplit[0]}");
                var dirname = dir;
                if (dirsplit.Length == 2)
                {
                    dirname = dirsplit[1];
                }
                //Debug.Log($"{dirname}:{dirsplit[0]}");
                allMapFiles.Add(new MapFiles(dirsplit[0], dirname, jsonFiles));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("GetAllLevels err" + e.ToString());
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
        isGameWin = false;
        currentLevel = allMapFiles[currentMapLevel].LevelFile.FindIndex(x => string.Equals(file, x.levelDir));
        loadMapFile = file;
        currentMapName = allMapFiles[currentMapLevel].LevelFile[currentLevel].showLevelName;
        TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Play");
    }


    private void OnAfterLoadScene()
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            MapManager.Instance.LoadMapFromFile(loadMapFile, currentMapName);
            if (currentMapLevel == 0 && currentLevel < 3)
            {
                MapManager.Instance.ShowTip(true);
            }
            else
            {
                MapManager.Instance.ShowTip(false);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Choose")
        {
            ChooseUI chooseUI = transform.Find("Canvas/Panel")?.GetComponent<ChooseUI>();
            if (chooseUI != null)
            {
                chooseUI.CurrentMapLevel = currentMapLevel;
            }
        }
    }
    private void OnStartLoadScene()
    {
        Player.Instance.transform.position = MapObject.hiddenPos;
    }
    public void WinGame()
    {
        MyCoroutines.StartCoroutine(MapManager.Instance.WinGame());
    }

    public bool HasNextLevel()
    {
        if (allMapFiles[currentMapLevel].LevelFile.Count > currentLevel + 1)
        {
            return true;
        }
        return false;
    }

    public bool HasNextBigLevel()
    {
        if (allMapFiles.Count > currentMapLevel + 1)
        {
            return true;
        }
        return false;
    }
    public void SetMapLevel(int maplevel)
    {
        currentMapLevel = maplevel;
        //MyLog.Log("SetMapLevel:" + currentMapLevel);
    }

    public void NextLevel()
    {
        currentLevel++;
        Debug.Log($"{currentLevel}:{allMapFiles[currentMapLevel].LevelFile[currentLevel].levelDir}");
        LoadMap(allMapFiles[currentMapLevel].LevelFile[currentLevel].levelDir);
    }
}
