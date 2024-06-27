using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChooseUI : MonoBehaviour
{
    private List<MapFiles> mapFiles = new List<MapFiles>();

    public GameObject levelPrefab;

    public Text levelText;
    public Button leftBtn;
    public Button rightBtn;

    private int currentMapLevel;
    public int CurrentMapLevel
    {
        get => currentMapLevel;
        set
        {
            currentMapLevel = value;
            GameManager.Instance.SetMapLevel(value);
        }
    }
    private void Awake()
    {
        mapFiles = GameManager.Instance.MapFiles;
    }
    void Start()
    {
        leftBtn.onClick.AddListener(delegate { ChangeMapLevel(-1); });
        rightBtn.onClick.AddListener(delegate { ChangeMapLevel(1); });
        SetMapLevel(0);
    }

    public void DrawAllLevels(int idx)
    {
        levelText.text = mapFiles[idx].LevelName;
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child == transform)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach (MapFile file in mapFiles[idx].LevelFile)
        {
            //Debug.Log(file);
            GameObject gb = Instantiate(levelPrefab, transform);
            Button b = gb.GetComponent<Button>();
            b.onClick.AddListener(delegate { ChooseLevel(file.levelDir); });
            Text t = b.GetComponentInChildren<Text>();
            if (t == null)
            {
                Debug.Log("er");
            }
            t.text = file.levelName;
        }
    }


    private void ChooseLevel(string levelPath)
    {
        GameManager.Instance.LoadMap(levelPath);
    }

    private void ChangeMapLevel(int delta)
    {
        SetMapLevel(CurrentMapLevel + delta);
    }

    private void SetMapLevel(int level)
    {
        if (level >= 0 && level < mapFiles.Count)
        {
            CurrentMapLevel = level;
        }
        DrawAllLevels(CurrentMapLevel);
        leftBtn.gameObject.SetActive(CurrentMapLevel != 0);
        rightBtn.gameObject.SetActive(CurrentMapLevel != mapFiles.Count - 1);
    }
}
