using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

#if UNITY_ANDROID              
// 定义文件夹树结构
[System.Serializable]
public class FolderNode
{
    public string name;                // 文件夹名称
    public List<string> files;         // 文件夹下的文件列表
    public List<FolderNode> subfolders; // 子文件夹列表
}
#endif

public class LevelManager
{
    public string loadMapFile;
    public string currentMapName;
    private List<MapFiles> allMapFiles = new List<MapFiles>();

    /// <summary>
    /// 当前关卡
    /// </summary>
    public int currentLevel { get; set; }
    /// <summary>
    /// 当前大关卡
    /// </summary>
    public int currentMapLevel { get; set; }

    public bool isAnimMoving = false;

    private string path;

    public LevelManager(string path)
    { 
        this.path = path;
    }

    public List<MapFiles> AllMapFiles
    {
        get => allMapFiles;
    }
    public IEnumerator GetAllLevelsInAndroid(string path)
    {
        string fileListPath = Path.Combine(path, "filelist.json");
        UnityWebRequest request = UnityWebRequest.Get(fileListPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("file Content: " + request.downloadHandler.text);
            try
            {
                FolderNode root = JsonConvert.DeserializeObject<FolderNode>(request.downloadHandler.text);
                List<MapFiles> mapFiles = GetFilesInFileNode(root);
                allMapFiles = mapFiles;
            }
            catch (Exception e)
            {
                Debug.LogError("GetAllLevelsInAndroid err" + e.ToString());
            }
        }
        else
        {
            Debug.Log("出现错误: " + request.error);
        }
    }

    private List<MapFiles> GetFilesInFileNode(FolderNode root)
    {
        List<MapFiles> mapFiles = new List<MapFiles>();
        while (root.subfolders.Count > 0)
        {
            root = root.subfolders[0];
            path = Path.Combine(path, root.name);
            for (int i = 0; i < root.subfolders.Count; i++)
            {
                string prefixDir = Path.Combine(path, root.subfolders[i].name);
                FolderNode node = root.subfolders[i];
                List<string> jsonFiles = new List<string>();
                foreach (var file in node.files)
                {
                    Debug.Log("File:" + file);
                    if (file.EndsWith("json"))
                    {
                        jsonFiles.Add(Path.Combine(prefixDir, file));
                        Debug.Log("Path:"+Path.Combine(prefixDir, file));
                    }
                }
                string dirname = node.name;
                var dirsplit = dirname.Split(' ', 2);
                Debug.Log($"{node.name}");
                if (dirsplit.Length == 2)
                {
                    dirname = dirsplit[1];
                }
                mapFiles.Add(new MapFiles(dirsplit[0], dirname, jsonFiles));
            }
        }
        return mapFiles;
    }

    public void GetAllLevelsInPC()
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
            Debug.LogError("GetAllLevelsInPC err" + e.ToString());
        }
    }

    public bool HasNextLevel()
    {
        if (AllMapFiles[currentMapLevel].LevelFile.Count > currentLevel + 1)
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
}

