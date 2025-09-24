using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    private int currentEnemies;
    [SerializeField] private Transform[] spawnPoints;

    void Start() { StartCoroutine(SpawnLoop()); }

    IEnumerator SpawnLoop()
    {
        var cfg = GameManager.Instance.Config;
        while (true)
        {
            int wait = cfg.m * currentEnemies + cfg.b;
            yield return new WaitForSeconds(wait);

            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Addressables.InstantiateAsync("Enemy", p.position, Quaternion.identity);
            currentEnemies++;
        }
    }

    public void OnEnemyDestroyed() => currentEnemies = Mathf.Max(0, currentEnemies - 1);
}
