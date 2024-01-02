using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseUI : MonoBehaviour
{
    private string path;
    private List<string> mapFiles = new List<string>();

    public GameObject levelPrefab;
    void Start()
    {
        path = Application.streamingAssetsPath;
        GetAllLevels();
        DrawAllLevels();
    }

    private void DrawAllLevels()
    {
        foreach (string file in mapFiles)
        {
            Debug.Log(file);
            GameObject gb = Instantiate(levelPrefab, transform);
            Button b = gb.GetComponent<Button>();
            b.onClick.AddListener(delegate { ChooseLevel(file); });
            Text t = b.GetComponentInChildren<Text>();
            if (t == null)
            {
                Debug.Log("er");
            }
            t.text = GetFileName(file);
        }
    }

    private string GetFileName(string file)
    {
        var f1 =file.Split('.');
        return f1[0].Split('\\')[1];
    }

    private void ChooseLevel(string levelPath)
    {
        GameManager.Instance.LoadMap(levelPath);
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
}
