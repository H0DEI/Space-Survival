using UnityEngine;

[RequireComponent(typeof(Health))]
public class DropExpOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject expOrbPrefab;
    [SerializeField] private int minOrbs = 1;
    [SerializeField] private int maxOrbs = 1;
    [SerializeField] private float scatterRadius = 0.6f;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath.AddListener(SpawnOrbs);
    }

    private void SpawnOrbs()
    {
        if (!expOrbPrefab) return;

        int count = Random.Range(minOrbs, maxOrbs + 1);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * scatterRadius;
            pos.y = transform.position.y;
            Instantiate(expOrbPrefab, pos, Quaternion.identity);
        }
    }
}
