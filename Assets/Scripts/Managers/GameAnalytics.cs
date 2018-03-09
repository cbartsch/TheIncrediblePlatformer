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
    private static extern void JS_Init();

    [DllImport("__Internal")]
    private static extern void JS_LevelFinished(int worldIndex, int levelIndex);

    [DllImport("__Internal")]
    private static extern void JS_GameFinished();

    [DllImport("__Internal")]
    private static extern void JS_Destroy();
#endif

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

#if UNITY_WEBGL
        JS_Init();
#else
        ga.StartSession();
        ga.LogScreen("Main");
        ga.LogEvent("general", "StartGame", "", 0);
#endif

        LogEvent("StartGame", null);
    }

    void OnDestroy()
    {
#if UNITY_EDITOR
        return;
#endif

#if UNITY_WEBGL
        JS_Destroy();
#else
        ga.StopSession();
#endif
    }

    void Update()
    {

    }

    public void OnKongregateAPILoaded(string userInfoString)
    {
        var info = userInfoString.Split('|');
        var userId = System.Convert.ToInt32(info[0]);
        var username = info[1];
        var gameAuthToken = info[2];
        Debug.Log("Kongregate User Info: " + username + ", userId: " + userId);
    }

    public void LevelFinished(int worldNum, int levelNum, TimeSpan timeDiff)
    {
#if UNITY_EDITOR
        return;
#endif
        Debug.Log("level finished event: " + (worldNum + 1) + "x" + (levelNum + 1));

        LogEvent("LevelFinished", new Dictionary<string, object>
        {
            {"WorldIndex", worldNum + 1},
            {"LevelIndex", levelNum + 1},
            {"Time", timeDiff.TotalMilliseconds}
        });


#if UNITY_WEBGL
        JS_LevelFinished(worldNum + 1, levelNum + 1);
#else
        ga.LogEvent("general", "LevelFinished_" + (worldNum + 1) + "_" + (levelNum + 1), "", (long)timeDiff.TotalMilliseconds);
#endif

        //if last level before credits was finished
        if (worldNum + 2 == GameManager.Instance.NumWorlds - 1 &&
            levelNum + 1 == GameManager.Instance.GetNumLevelsInWorld(worldNum - 1))
        {
            Debug.Log("game finished event");

            LogEvent("GameFinished", new Dictionary<string, object>());
#if UNITY_WEBGL
            JS_GameFinished();
#else
            ga.LogEvent("general", "GameFinished", "", 1);
#endif
        }
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
