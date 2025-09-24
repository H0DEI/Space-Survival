using UnityEngine;
using System.Collections;

public class ShockwaveBurst : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float interval = 3.0f;
    [SerializeField] private float damageDelay = 0.4f;  // time before damage happens
    [SerializeField] private float lingerTime = 0.6f;   // time the VFX stays after damage

    [Header("Damage")]
    [SerializeField] private float maxRadius = 5.0f;
    [SerializeField] private int damage = 3;
    [SerializeField] private LayerMask enemyMask = ~0;

    [Header("Visuals")]
    [SerializeField] private GameObject wavePrefab;
    [SerializeField] private bool scaleWaveToRadius = true; // scales XZ to diameter over time
    [SerializeField] private bool keepOriginalYScale = true;

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
        StartCoroutine(DoBurst());
    }

    private IEnumerator DoBurst()
    {
        Vector3 origin = transform.position;
        origin.y = 0f;

        GameObject vfx = null;
        if (wavePrefab != null)
        {
            vfx = Instantiate(wavePrefab, origin, Quaternion.identity);
            if (scaleWaveToRadius)
            {
                Vector3 s = vfx.transform.localScale;
                float y = keepOriginalYScale ? s.y : 1f;
                vfx.transform.localScale = new Vector3(0f, y, 0f);
                StartCoroutine(ScaleWaveOverTime(vfx.transform, damageDelay + lingerTime, y));
            }
        }

        if (damageDelay > 0f) yield return new WaitForSeconds(damageDelay);

        ApplyDamage(origin);

        if (lingerTime > 0f) yield return new WaitForSeconds(lingerTime);

        if (vfx != null) Destroy(vfx);
    }

    private IEnumerator ScaleWaveOverTime(Transform tf, float totalTime, float yScale)
    {
        if (totalTime <= 0f) yield break;

        float t0 = 0f;
        float targetDiameter = maxRadius * 2f;

        while (t0 < totalTime && tf != null)
        {
            t0 += Time.deltaTime;
            float k = Mathf.Clamp01(t0 / totalTime);
            tf.localScale = new Vector3(targetDiameter * k, yScale, targetDiameter * k);
            yield return null;
        }

        if (tf != null)
            tf.localScale = new Vector3(targetDiameter, yScale, targetDiameter);
    }

    private void ApplyDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, maxRadius, enemyMask);
        for (int i = 0; i < hits.Length; i++)
        {
            var h = hits[i].GetComponentInParent<Health>();
            if (h != null) h.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
