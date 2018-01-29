using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static int LevelNum
    {
        get { return PlayerPrefs.GetInt("LevelIndex", 0); }
        private set { PlayerPrefs.SetInt("LevelIndex", value); PlayerPrefs.Save(); }
    }

    public static int WorldNum
    {
        get { return PlayerPrefs.GetInt("WorldIndex", 0); }
        private set { PlayerPrefs.SetInt("WorldIndex", value); PlayerPrefs.Save(); }
    }

    public static bool SoundsEnabled
    {
        get { return PlayerPrefs.GetInt("SoundsEnabled", 1) != 0; }
        set { PlayerPrefs.SetInt("SoundsEnabled", value ? 1 : 0); PlayerPrefs.Save(); }
    }

    public static bool HasLevelData
    {
        get { return WorldNum > 0 || LevelNum > 0; }
    }

    public static void LevelReached(int worldIndex, int levelIndex)
    {
        if (worldIndex == WorldNum)
        {
            if (levelIndex > LevelNum) LevelNum = levelIndex;
        }

        if (worldIndex > WorldNum)
        {
            WorldNum = worldIndex;
            LevelNum = levelIndex;
        }

    }
}
