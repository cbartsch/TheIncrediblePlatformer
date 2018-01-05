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

    public List<GameObject> levels;

    public float respawnTime = 1;
    public float spawnInterval = 1;

    public Level level { get; private set; }

    private bool spawningPlayers = false;
    private int levelIndex = 0;
    private int activePlayers = 0;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        level = FindObjectOfType<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!level)
        {
            CreateLevel();
        }
        var players = FindObjectsOfType<Player>();
        if (players.Length == 0 && !spawningPlayers)
        {
            Collectible.ResetAll(level);
            activePlayers = level.playerData.Count;
            spawningPlayers = true;
            StartCoroutine(SpawnPlayers());
        }
    }

    private IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(respawnTime);

        foreach (var data in level.playerData)
        {
            var player = Instantiate(playerPrefab, level.spawnPoint.transform.position, Quaternion.identity, level.transform);
            var playerC = player.GetComponent<Player>();

            playerC.PlayerData = data;
            playerC.Level = level;

            yield return new WaitForSeconds(spawnInterval);
        }
        spawningPlayers = false;
    }

    public void GoalReached()
    {
        if (!level) { return; }

        activePlayers--;

        if (activePlayers == 0)
        {
            LevelFinished();
        }
    }

    private void LevelFinished()
    {
        ResetLevel();
        levelIndex = (levelIndex + 1) % levels.Count;
    }

    private void CreateLevel()
    {
        level = Instantiate(levels[levelIndex]).GetComponent<Level>();
        level.gameObject.SetActive(true);
    }

    public void ResetLevel()
    {
        Destroy(level.gameObject);
        level = null;
    }

    public void ResetPlayers()
    {
        foreach(var player in level.GetComponentsInChildren<Player>())
        {
            Destroy(player.gameObject);
        }
    }
}
