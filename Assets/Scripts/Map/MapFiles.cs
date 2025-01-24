using System;
using System.Collections.Generic;
using System.IO;

public class MapFiles : IComparable<MapFiles>
{
    private int levelIdx;
    private string levelfileName;
    private List<MapFile> levelfiles;
    public string LevelName { get => levelfileName; }
    public List<MapFile> LevelFile { get => levelfiles; }
    public MapFiles(string levelidx, string levelName, List<string> files)
    {
        this.levelIdx = int.Parse(levelidx);
        this.levelfileName = levelName;
        var levels = new List<MapFile>();
        foreach (var file in files)
        {
            levels.Add(new MapFile(file));
        }
        this.levelfiles = levels;
        levelfiles.Sort();
    }

    public int CompareTo(MapFiles obj)
    {
        return levelIdx - obj.levelIdx;
    }
}

public class MapFile : IComparable<MapFile>
{
    public string levelName;
    public string levelDir;
    public string showLevelName;
    public MapFile(string dir)
    {
        levelDir = dir;
        levelName = GetFileName(dir);
        showLevelName = GetShowFileName(levelDir);
        //MyLog.Log(showLevelName);
    }

    private string GetShowFileName(string file)
    {
        try
        {
            //MyLog.Log(file);
#if UNITY_ANDROID && !UNITY_EDITOR
            var f1 = file.Split('/');
#else
            var f1 = file.Split('\\');
#endif
            var filenames = f1[f1.Length - 2].Split(' ');
            //MyLog.Log(f1[f1.Length - 2]);
            if (filenames.Length == 3)
            {
                return filenames[2] + "-" + levelName;
            }
            return f1[0];
        }
        catch { return "Error-1"; }
    }

    public int CompareTo(MapFile mapFile)
    {
        int levelIdx = int.Parse(levelName);
        int levelIdx1 = int.Parse(mapFile.levelName);
        return levelIdx - levelIdx1;
    }

    private string GetFileName(string file)
    {
        var f1 = Path.GetFileName(file).Split('.');
        var filenames = f1[0].Split(' ');
        if (filenames.Length == 2)
        {
            return filenames[0];
        }
        return f1[0];
    }
}
