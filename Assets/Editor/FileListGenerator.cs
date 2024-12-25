using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HierarchicalFileListGenerator
{
    [MenuItem("Tools/Generate Hierarchical File List")]
    public static void GenerateFileList()
    {
        // 设置目标目录 (StreamingAssets 文件夹)
        string targetFolder = Application.dataPath + "/StreamingAssets";
        string outputFile = Path.Combine(targetFolder, "filelist.json");

        // 检查文件夹是否存在
        if (!Directory.Exists(targetFolder))
        {
            Debug.LogError("目标文件夹不存在: " + targetFolder);
            return;
        }

        // 生成文件夹树结构
        FolderNode root = new FolderNode { name = "StreamingAssets", files = new List<string>(), subfolders = new List<FolderNode>() };
        BuildFolderTree(targetFolder, root);

        // 转换为 JSON 格式
        string jsonContent = JsonUtility.ToJson(root, true);

        // 写入 JSON 文件
        File.WriteAllText(outputFile, jsonContent);
        Debug.Log("filelist.json 生成成功: " + outputFile);

        // 刷新 Unity 资源窗口
        AssetDatabase.Refresh();
    }

    // 递归构建文件夹树
    private static void BuildFolderTree(string currentPath, FolderNode currentNode)
    {
        // 获取当前文件夹下的所有文件
        string[] files = Directory.GetFiles(currentPath);
        foreach (string file in files)
        {
            // 过滤掉 .meta 文件
            if (!file.EndsWith(".meta"))
            {
                string fileName = Path.GetFileName(file);
                currentNode.files.Add(fileName);
            }
        }

        // 获取所有子文件夹
        string[] directories = Directory.GetDirectories(currentPath);
        foreach (string dir in directories)
        {
            FolderNode subfolder = new FolderNode
            {
                name = Path.GetFileName(dir),
                files = new List<string>(),
                subfolders = new List<FolderNode>()
            };

            // 递归处理子文件夹
            BuildFolderTree(dir, subfolder);

            // 添加到当前节点的子文件夹列表
            currentNode.subfolders.Add(subfolder);
        }
    }

    // 定义文件夹树结构
    [System.Serializable]
    private class FolderNode
    {
        public string name;                // 文件夹名称
        public List<string> files;         // 文件夹下的文件列表
        public List<FolderNode> subfolders; // 子文件夹列表
    }
}
