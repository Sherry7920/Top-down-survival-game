using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    // =========================
    // References
    // =========================
    [Header("References")]
    public Tilemap waterTilemap;          // Tilemap where monsters are allowed to spawn
    public GameObject monsterPrefab;      // Monster prefab to instantiate
    public Transform player;              // Player reference (must be assigned)

    // =========================
    // Initial Spawn
    // =========================
    [Header("Initial Spawn")]
    public int initialMonsterCount = 4;   // Monsters spawned at game start

    // =========================
    // Adaptive Spawn (Fake Learning)
    // =========================
    [Header("Adaptive Spawn (Fake Learning)")]
    public float checkInterval = 2f;      // How often the system evaluates player safety
    public float playerSafeRadius = 6f;   // Radius to check monster presence around player
    public int minMonsterAroundPlayer = 1;// Minimum threat level around the player

    // =========================
    // Spawn Control
    // =========================
    [Header("Spawn Control")]
    public float minSpawnDistanceFromPlayer = 2.5f; // Prevent spawning too close
    public float spawnCooldown = 3f;                // Minimum time between spawns

    float lastSpawnTime;

    void Start()
    {
        Debug.Log("[Spawner] Initializing Monster Spawner");

        SpawnInitialMonsters();
        StartCoroutine(AdaptiveSpawnLoop());
    }

    // =========================
    // Initial Monster Spawn
    // =========================
    void SpawnInitialMonsters()
    {
        List<Vector3> waterPositions = GetAllWaterPositions();

        Debug.Log("[Spawner] Found " + waterPositions.Count + " valid water tiles");

        for (int i = 0; i < initialMonsterCount && waterPositions.Count > 0; i++)
        {
            int index = Random.Range(0, waterPositions.Count);
            Vector3 pos = waterPositions[index];

            Instantiate(monsterPrefab, pos, Quaternion.identity);
            Debug.Log("[Spawner] Initial monster spawned at " + pos);

            waterPositions.RemoveAt(index);
        }
    }

    // =========================
    // Core Adaptive "Learning" Loop
    // =========================
    IEnumerator AdaptiveSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (Time.time < lastSpawnTime + spawnCooldown)
            {
                Debug.Log("[Spawner] Spawn on cooldown");
                continue;
            }

            int monsterCount = CountMonstersNearPlayer();
            Debug.Log("[Spawner] Monsters near player: " + monsterCount);

            // Player is too safe → increase pressure
            if (monsterCount < minMonsterAroundPlayer)
            {
                Debug.Log("[Spawner] Player is too safe → attempting to spawn");

                bool spawned = TrySpawnNearPlayer();

                if (spawned)
                {
                    lastSpawnTime = Time.time;
                    Debug.Log("[Spawner] Adaptive spawn SUCCESS");
                }
                else
                {
                    Debug.LogWarning("[Spawner] Adaptive spawn FAILED (no valid position)");
                }
            }
            else
            {
                Debug.Log("[Spawner] Threat level sufficient, no spawn needed");
            }
        }
    }

    // =========================
    // Count monsters around player
    // =========================
    int CountMonstersNearPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            player.position,
            playerSafeRadius,
            LayerMask.GetMask("Monster")
        );

        return hits.Length;
    }

    // =========================
    // Try spawning near the player (in water tiles)
    // =========================
    bool TrySpawnNearPlayer()
    {
        List<Vector3> validSpawns = new List<Vector3>();
        List<Vector3> waterPositions = GetAllWaterPositions();

        foreach (Vector3 pos in waterPositions)
        {
            float dist = Vector3.Distance(pos, player.position);

            if (dist <= playerSafeRadius && dist >= minSpawnDistanceFromPlayer)
            {
                validSpawns.Add(pos);
            }
        }

        Debug.Log("[Spawner] Valid spawn positions found: " + validSpawns.Count);

        if (validSpawns.Count == 0)
            return false;

        Vector3 spawnPos = validSpawns[Random.Range(0, validSpawns.Count)];
        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);

        Debug.Log("[Spawner] Monster spawned at " + spawnPos +
                  " | Distance from player: " +
                  Vector3.Distance(spawnPos, player.position).ToString("F2"));

        return true;
    }

    // =========================
    // Get all water tile world positions
    // =========================
    List<Vector3> GetAllWaterPositions()
    {
        BoundsInt bounds = waterTilemap.cellBounds;
        TileBase[] tiles = waterTilemap.GetTilesBlock(bounds);

        List<Vector3> positions = new List<Vector3>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tiles[x + y * bounds.size.x];
                if (tile == null) continue;

                Vector3Int cellPos = new Vector3Int(
                    bounds.x + x,
                    bounds.y + y,
                    0
                );

                Vector3 worldPos = waterTilemap.CellToWorld(cellPos)
                                   + waterTilemap.tileAnchor;

                positions.Add(worldPos);
            }
        }

        return positions;
    }

    // =========================
    // Debug Visualization
    // =========================
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Player safety evaluation radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, playerSafeRadius);

        // Minimum spawn distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minSpawnDistanceFromPlayer);
    }
}