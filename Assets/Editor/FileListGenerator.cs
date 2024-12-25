using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HierarchicalFileListGenerator
{
    [MenuItem("Tools/Generate Hierarchical File List")]
    public static void GenerateFileList()
    {
        // ����Ŀ��Ŀ¼ (StreamingAssets �ļ���)
        string targetFolder = Application.dataPath + "/StreamingAssets";
        string outputFile = Path.Combine(targetFolder, "filelist.json");

        // ����ļ����Ƿ����
        if (!Directory.Exists(targetFolder))
        {
            Debug.LogError("Ŀ���ļ��в�����: " + targetFolder);
            return;
        }

        // �����ļ������ṹ
        FolderNode root = new FolderNode { name = "StreamingAssets", files = new List<string>(), subfolders = new List<FolderNode>() };
        BuildFolderTree(targetFolder, root);

        // ת��Ϊ JSON ��ʽ
        string jsonContent = JsonUtility.ToJson(root, true);

        // д�� JSON �ļ�
        File.WriteAllText(outputFile, jsonContent);
        Debug.Log("filelist.json ���ɳɹ�: " + outputFile);

        // ˢ�� Unity ��Դ����
        AssetDatabase.Refresh();
    }

    // �ݹ鹹���ļ�����
    private static void BuildFolderTree(string currentPath, FolderNode currentNode)
    {
        // ��ȡ��ǰ�ļ����µ������ļ�
        string[] files = Directory.GetFiles(currentPath);
        foreach (string file in files)
        {
            // ���˵� .meta �ļ�
            if (!file.EndsWith(".meta"))
            {
                string fileName = Path.GetFileName(file);
                currentNode.files.Add(fileName);
            }
        }

        // ��ȡ�������ļ���
        string[] directories = Directory.GetDirectories(currentPath);
        foreach (string dir in directories)
        {
            FolderNode subfolder = new FolderNode
            {
                name = Path.GetFileName(dir),
                files = new List<string>(),
                subfolders = new List<FolderNode>()
            };

            // �ݹ鴦�����ļ���
            BuildFolderTree(dir, subfolder);

            // ��ӵ���ǰ�ڵ�����ļ����б�
            currentNode.subfolders.Add(subfolder);
        }
    }

    // �����ļ������ṹ
    [System.Serializable]
    private class FolderNode
    {
        public string name;                // �ļ�������
        public List<string> files;         // �ļ����µ��ļ��б�
        public List<FolderNode> subfolders; // ���ļ����б�
    }
}
