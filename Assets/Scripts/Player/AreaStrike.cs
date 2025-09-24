using UnityEngine;
using System.Collections;

public class AreaStrike : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float searchRadius = 25f;
    [SerializeField] private LayerMask enemyMask = ~0;

    [Header("Timing")]
    [SerializeField] private float interval = 2.5f;
    [SerializeField] private float damageDelay = 0.6f;  // time before damage happens
    [SerializeField] private float lingerTime = 1.2f;   // time the area stays after damage

    [Header("Damage")]
    [SerializeField] private float aoeRadius = 3.5f;
    [SerializeField] private int damage = 2;

    [Header("Visuals")]
    [SerializeField] private GameObject areaPrefab;     // your VFX circle prefab
    [SerializeField] private bool scaleAreaToRadius = true;

    private float t;

    void OnEnable()
    {
        t = interval; // fire immediately on enable
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t < interval) return;
        t = 0f;

        Transform target = FindClosestEnemy();
        if (target != null)
            StartCoroutine(StrikeSequence(target.position));
    }

    private IEnumerator StrikeSequence(Vector3 pos)
    {
        Vector3 p = pos; p.y = 0f;

        GameObject area = null;
        if (areaPrefab != null)
        {
            area = Instantiate(areaPrefab, p, Quaternion.identity);
            if (scaleAreaToRadius)
                area.transform.localScale = new Vector3(aoeRadius * 2f, 1f, aoeRadius * 2f);
        }

        if (damageDelay > 0f)
            yield return new WaitForSeconds(damageDelay);

        ApplyDamage(p);

        if (lingerTime > 0f)
            yield return new WaitForSeconds(lingerTime);

        if (area != null)
            Destroy(area);
    }

    private void ApplyDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, aoeRadius, enemyMask);
        for (int i = 0; i < hits.Length; i++)
        {
            var h = hits[i].GetComponentInParent<Health>();
            if (h != null)
                h.TakeDamage(damage);
        }
    }

    private Transform FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, enemyMask);
        float best = float.PositiveInfinity;
        Transform bestT = null;

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].CompareTag("Enemy")) continue;
            float d = (hits[i].transform.position - transform.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                bestT = hits[i].transform;
            }
        }
        return bestT;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
