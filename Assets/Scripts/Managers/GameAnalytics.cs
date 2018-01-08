using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : MonoBehaviour
{
    public static GameAnalytics Instance { get; private set; }

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
            {"WorldNum", worldNum},
            {"LevelNum", levelNum},
            {"Time", timeDiff.TotalMilliseconds}
        });
    }

    private void LogEvent(string eventName, Dictionary<string, object> eventParams)
    {
        Debug.Log("[Analytics] Log event " + eventName);
        Analytics.CustomEvent(eventName, eventParams);
        Amplitude.Instance.logEvent(eventName, eventParams);
    }
}
