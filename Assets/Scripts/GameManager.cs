using MyInputSystem;
using MyTool.Music;
using MyTools.MyCoroutines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    private string path;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public List<MapFiles> AllMapFiles { get => levelManager.AllMapFiles; }

    public string StartSceneName;

    private LevelManager levelManager;

    public bool isGameWin;
    public int currentMapLevel { get => levelManager.currentMapLevel; }
    public int currentLevel { get => levelManager.currentLevel; }
    public bool GridOn { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string androidPath = Application.streamingAssetsPath;
        path = androidPath;
        levelManager = new LevelManager(path);
        Debug.Log(path);
        //path = Application.streamingAssetsPath;
        //GetAllLevelsInPC();
        StartCoroutine(levelManager.GetAllLevelsInAndroid(path));
#else
        StartUI.SetFullScreen();
        path = Path.Combine(Application.streamingAssetsPath, "Levels");
        Debug.Log(path);
        levelManager = new LevelManager(path);
        levelManager.GetAllLevelsInPC();
#endif

        TransitionManager.Instance.Transition("", StartSceneName);
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
        levelManager.currentLevel = levelManager.AllMapFiles[levelManager.currentMapLevel].
            LevelFile.FindIndex(x => string.Equals(file, x.levelDir));
        levelManager.loadMapFile = file;
        levelManager.currentMapName = levelManager.AllMapFiles[levelManager.currentMapLevel].
            LevelFile[levelManager.currentLevel].showLevelName;
        TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Play");
    }


    private void OnAfterLoadScene(object sender, EventArgs eventArgs)
    {
        TransitionEventArgs myEventArgs = eventArgs as TransitionEventArgs;
        if (myEventArgs == null)
        {
            MyLog.LogError("OnAfterLoadScene :EventArgs is null");
            return;
        }
        if (myEventArgs.to == "Play")
        {
            MapManager.Instance.LoadMapFromFile(levelManager.loadMapFile, levelManager.currentMapName);
#if UNITY_STANDALONE
            if (currentMapLevel == 0 && currentLevel < 3)
            {
                MapManager.Instance.ShowTip(true);
            }
            else
            {
                MapManager.Instance.ShowTip(false);
            }
            MapManager.Instance.ShowInput(false);
#elif UNITY_ANDROID
            MapManager.Instance.ShowInput(true);
#endif
        }
        else if (myEventArgs.to == "Choose")
        {
            ChooseUI chooseUI = transform.Find("Canvas/Panel")?.GetComponent<ChooseUI>();
            if (chooseUI != null)
            {
                chooseUI.CurrentMapLevel = levelManager.currentMapLevel;
            }
        }
    }
    private void OnStartLoadScene(object sender, EventArgs eventArgs)
    {
        Player.Instance.transform.position = MapObject.hiddenPos;
    }
    public void WinGame()
    {
        MusicManager.Instance.PlayEffect(MusicEnum.Win);
        MyCoroutines.StartCoroutine(MapManager.Instance.WinGame());
    }

    public bool HasNextLevel()
    {
        return levelManager.HasNextLevel();
    }

    public bool HasNextBigLevel()
    {
        return levelManager.HasNextBigLevel();
    }
    public void SetMapLevel(int maplevel)
    {
        levelManager.SetMapLevel(maplevel);
    }

    public void NextLevel()
    {
        levelManager.currentLevel++;
        Debug.Log($"{levelManager.currentLevel}:{levelManager.AllMapFiles[levelManager.currentMapLevel].LevelFile[levelManager.currentLevel].levelDir}");
        LoadMap(levelManager.AllMapFiles[levelManager.currentMapLevel].LevelFile[levelManager.currentLevel].levelDir);
    }

    public void ShowGrid()
    {
        MapManager.Instance.ShowGrid();
    }
}
