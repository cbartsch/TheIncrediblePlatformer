using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : MonoBehaviour
{
    public static GameAnalytics Instance { get; private set; }

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

        Amplitude.Instance.logEvent("StartGame");

        Analytics.enabled = true;
        Analytics.deviceStatsEnabled = true;
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
