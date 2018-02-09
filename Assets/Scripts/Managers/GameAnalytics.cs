using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : MonoBehaviour
{
    public static GameAnalytics Instance { get; private set; }

    public GoogleAnalyticsV4 ga;

    void Start()
    {
        Instance = this;

#if UNITY_EDITOR
        return;
#endif
        Amplitude amplitude = Amplitude.Instance;
        amplitude.logging = true;
        amplitude.init("451e76614d15535b86777ebd2bf87125");

        Analytics.enabled = true;
        Analytics.deviceStatsEnabled = true;

        ga.LogScreen("Main");

        LogEvent("StartGame", null);
        ga.LogEvent("general", "StartGame", "", 0);
    }

    void Update()
    {

    }

    public void LevelFinished(int worldNum, int levelNum, TimeSpan timeDiff)
    {
#if UNITY_EDITOR
        return;
#endif

        LogEvent("LevelFinished", new Dictionary<string, object>
        {
            {"WorldIndex", worldNum + 1},
            {"LevelIndex", levelNum + 1},
            {"Time", timeDiff.TotalMilliseconds}
        });

        ga.LogEvent("general", "LevelFinished_" + (worldNum + 1) + "_" + (levelNum + 1), "", (long)timeDiff.TotalMilliseconds);
    }

    private void LogEvent(string eventName, Dictionary<string, object> eventParams)
    {
#if UNITY_EDITOR
        return;
#endif

        Debug.Log("[Analytics] Log event " + eventName);
        Analytics.CustomEvent(eventName, eventParams);
        Amplitude.Instance.logEvent(eventName, eventParams);
    }
}
