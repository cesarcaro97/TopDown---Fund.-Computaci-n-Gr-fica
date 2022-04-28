using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab = null;
    [SerializeField] private GameObject wallPrefab = null;
    [SerializeField] private GameObject startPositionPrefab = null;
    [SerializeField] private GameObject endPositionPrefab = null;
    [SerializeField] private GameObject playerSpawner = null;
    [SerializeField] private GameObject enemiesSpawner = null;

    public GameObject GenerateLevel(LevelInfo level)
    {
        var levelHolder = new GameObject("Level");
        for (int x = 0; x < level.Width; x++)
        {
            for (int y = 0; y < level.Height; y++)
            {
                string pos = level.TilesMap[x, y];
                var prefabs = GetPositionPrefab(pos);
                foreach (var p in prefabs)
                {
                    Instantiate(p, Vector2.right * x + Vector2.up * y, Quaternion.identity, levelHolder.transform);
                }
            }
        }

        return levelHolder;
    }

    public IEnumerator IGenerateLevel(LevelInfo level)
    {
        var levelHolder = new GameObject("Level");
        for (int x = 0; x < level.Width; x++)
        {
            for (int y = 0; y < level.Height; y++)
            {
                string pos = level.TilesMap[x, y];
                var prefabs = GetPositionPrefab(pos);
                foreach (var p in prefabs)
                {
                    Instantiate(p, Vector2.right * x + Vector2.up * y, Quaternion.identity, levelHolder.transform);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerable<GameObject> GetPositionPrefab(string tileCode)
    {
        List<GameObject> prefabs = new List<GameObject>();
        switch (tileCode)
        {
            case TilesCodes.GROUND:
                prefabs.Add(groundPrefab);
                break;
            case TilesCodes.WALL:
                prefabs.Add(wallPrefab);
                break;
            case TilesCodes.START_POS:
                prefabs.Add(startPositionPrefab);
                prefabs.Add(playerSpawner);
                break;
            case TilesCodes.END_POS:
                prefabs.Add(endPositionPrefab);
                break;
            case TilesCodes.ENEMIES_SPAWN:
                prefabs.Add(groundPrefab);
                prefabs.Add(enemiesSpawner);
                break;
        }

        return prefabs;
    }
}

public static class TilesCodes
{
    public const string GROUND = "0";
    public const string WALL = "1";
    public const string START_POS = "2";
    public const string END_POS = "3";
    public const string ENEMIES_SPAWN = "4";
}
