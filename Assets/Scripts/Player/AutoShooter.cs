using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireInterval = 0.4f;
    [SerializeField] private float searchRadius = 30f;
    [SerializeField] private LayerMask enemyMask;

    private float _t;
    private AutoAimAtEnemy aimer;

    void Awake()
    {
        aimer = GetComponent<AutoAimAtEnemy>();
    }

    void Update()
    {
        _t += Time.deltaTime;
        if (_t < fireInterval) return;
        _t = 0f;

        Transform target = FindClosestEnemy();
        if (!target) return;

        if (aimer != null)
            aimer.TriggerAim();

        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;
        var p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        p.GetComponent<Projectile>().Launch(dir);
    }

    private Transform FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, enemyMask);
        float best = float.PositiveInfinity;
        Transform bestT = null;

        foreach (var h in hits)
        {
            if (!h.CompareTag("Enemy")) continue;
            float d = (h.transform.position - transform.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                bestT = h.transform;
            }
        }
        return bestT;
    }
}
