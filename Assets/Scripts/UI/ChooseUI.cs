using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChooseUI : MonoBehaviour
{
    private List<string> mapFiles = new List<string>();

    public GameObject levelPrefab;
    private void Awake()
    {
        mapFiles = GameManager.Instance.MapFiles;
    }
    void Start()
    {
        DrawAllLevels();
    }

    public void DrawAllLevels()
    {
        foreach (string file in mapFiles)
        {
            //Debug.Log(file);
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


}
