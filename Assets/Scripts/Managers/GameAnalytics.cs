using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : MonoBehaviour
{
    public static GameAnalytics Instance { get; private set; }

    public GoogleAnalyticsV4 ga;

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void JS_LevelFinished(int worldIndex, int levelIndex);
#endif

    void Start()
    {
        Instance = this;

        Amplitude amplitude = Amplitude.Instance;
        amplitude.logging = true;
        amplitude.init("451e76614d15535b86777ebd2bf87125");

        LogEvent("StartGame", null);

        Analytics.enabled = true;
        Analytics.deviceStatsEnabled = true;

        ga.LogEvent("general", "StartGame", "", 0);
    }
    
    void Update()
    {

    }

    public void LevelFinished(int worldNum, int levelNum, TimeSpan timeDiff)
    {
        LogEvent("LevelFinished", new Dictionary<string, object>
        {
            {"WorldIndex", worldNum + 1},
            {"LevelIndex", levelNum + 1},
            {"Time", timeDiff.TotalMilliseconds}
        });

        ga.LogEvent("general", "LevelFinished_" + (worldNum + 1) + "_" + (levelNum + 1), "", (long)timeDiff.TotalMilliseconds);

#if UNITY_WEBGL && !UNITY_EDITOR
        JS_LevelFinished(worldNum + 1, levelNum + 1);
#endif
    }

    private void LogEvent(string eventName, Dictionary<string, object> eventParams)
    {
        Debug.Log("[Analytics] Log event " + eventName);
        Analytics.CustomEvent(eventName, eventParams);
        Amplitude.Instance.logEvent(eventName, eventParams);
    }
}
