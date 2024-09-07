using UnityEngine;


public static class MyLog
{
    public static void Log(string message)
    {
        Debug.Log("Current time: " +
            System.DateTime.Now.ToString("HH: mm:ss.fff")
            + " " + message);
    }
}

