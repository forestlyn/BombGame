using System;
using System.Collections.Generic;
using System.IO;

public class MapFiles: IComparable<MapFiles>
{
    private int levelIdx;
    private string levelfileName;
    private List<MapFile> levelfiles;
    public string LevelName { get => levelfileName; }
    public List<MapFile> LevelFile { get => levelfiles; }
    public MapFiles(string levelidx,string levelName, List<string> files)
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

public class MapFile: IComparable<MapFile>
{
    public string levelName;
    public string levelDir;

    public MapFile(string dir)
    {
        levelDir = dir;
        levelName = GetFileName(dir);
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
