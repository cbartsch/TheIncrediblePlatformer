using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDecorator : MonoBehaviour
{
    public static LevelDecorator Instance { get; private set; }

    public List<GameObject> decorPrefabs;

    void Awake()
    {
        Instance = this;
    }

    public void Decorate(Level level)
    {
        for (int i = 0; i < Random.Range(level.minDecors, level.maxDecors); i++)
        {
            SpawnDecor(level);
        }
    }

    private void SpawnDecor(Level level)
    {
        //generate random decor object from list at random position
        var decorPrefab = decorPrefabs[Random.Range(0, decorPrefabs.Count)];
        var bounds = level.levelBounds;

        //only spawn in top part of level as bottom half has "mountain" background
        var minY = bounds.min.y + bounds.size.y * 0.3f;
        //can also be a bit on top of level
        var maxY = bounds.max.y + bounds.size.y * 0.4f;

        var pos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(        minY,         maxY),
            Random.Range(bounds.min.z, bounds.max.z)
            );

        var obj = Instantiate(decorPrefab, pos, Quaternion.identity, level.transform);
    }
}
