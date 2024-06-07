using System.Collections.Generic;

public class MapFile
{
    private string levelName;
    private List<string> levelfiles;
    public string LevelName { get => levelName; }
    public List<string> LevelFile { get => levelfiles; }
    public MapFile(string levelName, List<string> files)
    {
        this.levelName = levelName;
        this.levelfiles = files;
    }
}