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
    public float spawnStartTime = 0.5f;

    public Level level { get; private set; }

    private bool spawningPlayers = false;

    //the annotations make the fields be visible in the editor, for testing
    [SerializeField]
    private int levelIndex = 0;
    [SerializeField]
    private int worldIndex = -1;

    private int activePlayers = 0;
    private DateTime levelStartTime;

    private Coroutine spawnRoutine;

    private bool wasPaused;

    public int WorldIndex { get { return worldIndex; } }
    public int LevelIndex { get { return levelIndex; } }

    private int NumWorlds { get { return levelContainer.transform.childCount; } }
    private int NumLevelsInWorld { get { return GetNumLevelsInWorld(this.worldIndex); } }

    public int GetNumLevelsInWorld(int index)
    {
        return levelContainer.transform.GetChild(index + 1).childCount;
    }

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
            Resettable.ResetAll(level);
        }

        if (!level)
        {
            CreateLevel();
        }
        if (players.Length == 0 && !spawningPlayers && !isPaused)
        {
            Resettable.ResetAll(level);
            activePlayers = level.GoalPlayerCount;
            spawningPlayers = true;

            spawnRoutine = StartCoroutine(SpawnPlayers());
        }

        wasPaused = isPaused;
    }

    private IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(spawnStartTime);

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

        Debug.Log("goal reached, active players:" + activePlayers);
        if (activePlayers == 0)
        {
            LevelFinished(isContinueGoal:isContinueGoal);
        }
    }

    private void LevelFinished(bool isContinueGoal = false)
    {
        var timeDiff = DateTime.Now - levelStartTime;
        GameAnalytics.Instance.LevelFinished(WorldIndex, LevelIndex, timeDiff);

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

            if (worldIndex + 1 >= NumWorlds)
            {
                worldIndex = -1;
            }
        }

        Persistence.LevelReached(worldIndex, levelIndex);
    }

    private void CreateLevel()
    {
        var world = levelContainer.transform.GetChild(this.worldIndex + 1);
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
        Resettable.ResetAll(level, resetLevelPosition: true);
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

        activePlayers = level.GoalPlayerCount;

        foreach (var player in level.GetComponentsInChildren<Player>())
        {
            player.Remove();
        }
    }
}
