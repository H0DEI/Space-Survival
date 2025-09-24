using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    private int currentEnemies;
    [SerializeField] private Transform[] spawnPoints;

    // Difficulty scaling
    [SerializeField] private float difficultyMultiplier = 0.3f;   // starts at normal speed
    [SerializeField] private float acceleration = 0.9f;        // each spawn is 2% faster (adjustable)
    [SerializeField] private float minWait = 0.1f;              // minimum delay between spawns

    void Start() { StartCoroutine(SpawnLoop()); }

    IEnumerator SpawnLoop()
    {
        var cfg = GameManager.Instance.Config;
        while (true)
        {
            // Original formula (kept for reference):
            // float wait = cfg.m * currentEnemies + cfg.b;

            // New formula with difficulty multiplier
            float wait = Mathf.Max(minWait, (cfg.m * currentEnemies + cfg.b) * difficultyMultiplier);

            yield return new WaitForSeconds(wait);

            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Addressables.InstantiateAsync("Enemy", p.position, Quaternion.identity);
            currentEnemies++;

            // Apply progressive acceleration
            difficultyMultiplier *= acceleration;
        }
    }

    public void OnEnemyDestroyed() => currentEnemies = Mathf.Max(0, currentEnemies - 1);
}
