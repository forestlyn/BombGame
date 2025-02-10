using UnityEngine;


public static class MyLog
{
    public static void Log(string message)
    {
        Debug.Log(message);
    }
    public static void LogWithTime(string message)
    {
        Debug.Log("Current time: " +
            System.DateTime.Now.ToString("HH: mm:ss.fff")
            + " " + message);
    }

    public static void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    public static void LogError(string message)
    {
        Debug.LogError(message);
    }
}

