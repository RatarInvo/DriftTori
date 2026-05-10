using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance;

    [Header("References")]
    public Transform gridParent;
    public GameObject coinPrefab;

    [Header("Spawn Settings")]
    public int minCoins = 3;
    public int maxCoins = 6;
    public float minDistanceFromSpawn = 4f;
    public int tileSafetyMargin = 1;
    public int maxFloodFillCells = 15000;

    List<GameObject> activeCoins = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnCoinsForLevel(int levelIndex, Vector2 spawnPoint)
    {
        ClearCoins();

        if (gridParent == null || levelIndex >= gridParent.childCount) return;

        Transform levelParent = gridParent.GetChild(levelIndex);
        Transform wallsParent = levelParent.Find("Walls");
        if (wallsParent == null)
        {
            Debug.LogWarning($"No Walls child in level {levelIndex}");
            return;
        }

        Tilemap wallTilemap = wallsParent.GetComponentInChildren<Tilemap>();
        if (wallTilemap == null)
        {
            Debug.LogWarning($"No Tilemap found in Walls for level {levelIndex}");
            return;
        }

        List<Vector2> validPositions = GetTrackPositions(wallTilemap, spawnPoint, minDistanceFromSpawn);
        Debug.Log($"Found {validPositions.Count} valid coin positions on track");

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No valid positions found — check tileSafetyMargin or spawn point");
            return;
        }

        Shuffle(validPositions);
        int coinCount = Mathf.Min(Random.Range(minCoins, maxCoins + 1), validPositions.Count);

        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, validPositions[i], Quaternion.identity);
            activeCoins.Add(coin);
        }

        Debug.Log($"Spawned {coinCount} coins for level {levelIndex}");
    }

    List<Vector2> GetTrackPositions(Tilemap wallTilemap, Vector2 spawnPoint, float minDist)
    {
        List<Vector2> validPositions = new List<Vector2>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        Vector3Int startCell = wallTilemap.WorldToCell(new Vector3(spawnPoint.x, spawnPoint.y, 0));
        queue.Enqueue(startCell);
        visited.Add(startCell);

        Vector3 cellCenter = wallTilemap.cellSize * 0.5f;

        while (queue.Count > 0 && visited.Count < maxFloodFillCells)
        {
            Vector3Int current = queue.Dequeue();

            Vector3Int[] neighbours = {
                current + Vector3Int.right,
                current + Vector3Int.left,
                current + Vector3Int.up,
                current + Vector3Int.down
            };

            foreach (Vector3Int neighbour in neighbours)
            {
                if (visited.Contains(neighbour)) continue;
                if (wallTilemap.HasTile(neighbour)) continue;

                visited.Add(neighbour);
                queue.Enqueue(neighbour);
            }

            if (IsSafeFromWalls(wallTilemap, current))
            {
                Vector3 worldPos = wallTilemap.CellToWorld(current) + cellCenter;

                // Skip if too close to spawn point
                if (Vector2.Distance(new Vector2(worldPos.x, worldPos.y), spawnPoint) < minDist)
                    continue;

                validPositions.Add(new Vector2(worldPos.x, worldPos.y));
            }
        }

        return validPositions;
    }

    bool IsSafeFromWalls(Tilemap tilemap, Vector3Int cell)
    {
        for (int dx = -tileSafetyMargin; dx <= tileSafetyMargin; dx++)
        {
            for (int dy = -tileSafetyMargin; dy <= tileSafetyMargin; dy++)
            {
                if (tilemap.HasTile(cell + new Vector3Int(dx, dy, 0)))
                    return false;
            }
        }
        return true;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void ClearCoins()
    {
        foreach (GameObject coin in activeCoins)
            if (coin != null) Destroy(coin);
        activeCoins.Clear();
    }
}