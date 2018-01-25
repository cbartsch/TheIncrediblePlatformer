using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }

    public GameObject playerPrefab;

    public GameObject levelContainer;

    public float respawnTime = 1;
    public float spawnInterval = 1;

    public Level level { get; private set; }

    private bool spawningPlayers = false;
    private int levelIndex = 0;
    private int worldIndex = 0;
    private int activePlayers = 0;
    private DateTime levelStartTime;

    private Coroutine spawnRoutine;

    private bool wasPaused;

    //TODO
    public int WorldNum { get { return 1; } }

    public int LevelNum { get { return levelIndex + 1; } }

    private int NumWorlds { get { return levelContainer.transform.childCount; } }
    private int NumLevelsInWorld { get { return levelContainer.transform.GetChild(worldIndex).childCount; } }

    public bool Paused { get; set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        level = FindObjectOfType<Level>();
    }

    void Update()
    {
        var isPaused = Paused || DragController.DragActive;

        var players = FindObjectsOfType<Player>();
        Time.timeScale = isPaused ? 0 : 1;
        if (isPaused && !wasPaused)
        {
            ResetPlayers();
            MoveItem.ResetAll(level);
        }

        if (!level)
        {
            CreateLevel();
        }
        if (players.Length == 0 && !spawningPlayers && !isPaused)
        {
            MoveItem.ResetAll(level);
            activePlayers = level.playerData.Count;
            spawningPlayers = true;

            spawnRoutine = StartCoroutine(SpawnPlayers());
        }

        wasPaused = isPaused;
    }

    private IEnumerator SpawnPlayers()
    {
        foreach (var data in level.playerData)
        {
            var player = Instantiate(playerPrefab, level.spawnPoint.transform.position, Quaternion.identity, level.transform);
            var playerC = player.GetComponent<Player>();

            playerC.GoalReached += GoalReached;
            playerC.PlayerData = data;
            playerC.Level = level;

            yield return new WaitForSeconds(spawnInterval);
        }
        spawningPlayers = false;

        spawnRoutine = null;
    }

    public void GoalReached(bool isContinueGoal)
    {
        if (!level) { return; }

        activePlayers--;

        if (activePlayers == 0)
        {
            LevelFinished(isContinueGoal:isContinueGoal);
        }
    }

    private void LevelFinished(bool isContinueGoal = false)
    {
        var timeDiff = DateTime.Now - levelStartTime;
        GameAnalytics.Instance.LevelFinished(WorldNum, LevelNum, timeDiff);

        DestroyLevel();

        if (isContinueGoal)
        {
            worldIndex = Persistence.WorldNum;
            levelIndex = Persistence.LevelNum;
        }
        else
        {
            levelIndex++;
            if (levelIndex >= NumLevelsInWorld)
            {
                levelIndex = 0;
                worldIndex++;
            }

            if (worldIndex >= NumWorlds)
            {
                worldIndex = 0;
            }
        }

        Persistence.LevelReached(worldIndex, levelIndex);
    }

    private void CreateLevel()
    {
        var world = levelContainer.transform.GetChild(this.worldIndex);
        var levelTemplate = world.transform.GetChild(this.levelIndex);
        this.level = Instantiate(levelTemplate.gameObject).GetComponent<Level>();
        this.level.gameObject.SetActive(true);
        levelStartTime = DateTime.Now;
    }

    //hard reset: destroy level (use when switching to another level)
    public void DestroyLevel()
    {
        Destroy(level.gameObject);
        level = null;
    }

    //soft reset: reset all objects in level
    public void ResetLevel()
    {
        ResetPlayers();
        MoveItem.ResetAll(level, resetDropPosition: true);
    }

    public void TogglePause()
    {
        Paused = !Paused;
        Debug.Log("paused:" + Paused);
    }

    public void ResetPlayers()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawningPlayers = false;
        }

        activePlayers = level.playerData.Count;

        foreach (var player in level.GetComponentsInChildren<Player>())
        {
            player.Remove();
        }
    }
}
