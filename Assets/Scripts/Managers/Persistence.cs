using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{

    public static int LevelNum
    {
        get { return PlayerPrefs.GetInt("LevelNum", 0); }
        set { PlayerPrefs.SetInt("LevelNum", value); PlayerPrefs.Save(); }
    }

    public static int WorldNum
    {
        get { return PlayerPrefs.GetInt("WorldNum", 0); }
        set { PlayerPrefs.SetInt("WorldNum", value); PlayerPrefs.Save(); }
    }
}
