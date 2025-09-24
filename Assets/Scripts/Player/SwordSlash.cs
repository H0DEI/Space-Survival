using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float interval = 1.2f;

    [Header("Hit shape")]
    [SerializeField] private float range = 3.0f;           // max distance from player
    [SerializeField] private float arcAngleDeg = 100f;      // total cone angle
    [SerializeField] private float sphereRadius = 2.0f;     // overlap helper radius
    [SerializeField] private float forwardOffset = 1.0f;    // centers the sweep forward
    [SerializeField] private LayerMask enemyMask = ~0;

    [Header("Damage")]
    [SerializeField] private int damage = 2;

    [Header("Visuals")]
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float slashLifetime = 0.6f;
    [SerializeField] private bool parentSlashToPlayer = true;
    [SerializeField] private bool scaleSlashToRange = true;

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
        DoSlash();
    }

    private void DoSlash()
    {
        Vector3 origin = transform.position;
        Vector3 fwd = transform.forward;

        // small lift to avoid z fighting with ground
        float vfxYOffset = 0.02f;

        // Damage first or VFX first is fine, but VFX should not rely on physics
        // 1. spawn and parent first, without keeping world transform
        if (slashPrefab != null)
        {
            var vfx = Instantiate(slashPrefab);
            if (parentSlashToPlayer) vfx.transform.SetParent(transform, false);

            // 2. set world pose explicitly
            Vector3 worldPos = new Vector3(origin.x, origin.y + vfxYOffset, origin.z) + fwd * forwardOffset;
            vfx.transform.position = worldPos;
            vfx.transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);

            // 3. scale in world space on XZ so it looks consistent even if parent has scaling
            if (scaleSlashToRange)
            {
                Vector3 parentScale = vfx.transform.parent ? vfx.transform.parent.lossyScale : Vector3.one;
                Vector3 s = vfx.transform.localScale;
                float x = range / Mathf.Max(0.0001f, parentScale.x);
                float z = range / Mathf.Max(0.0001f, parentScale.z);
                vfx.transform.localScale = new Vector3(x, s.y, z);
            }

            if (slashLifetime > 0f) Destroy(vfx, slashLifetime);
        }

        // Hit detection
        Vector3 center = origin + fwd * forwardOffset;
        Collider[] hits = Physics.OverlapSphere(center, sphereRadius, enemyMask);
        float halfAngle = arcAngleDeg * 0.5f;
        float rangeSqr = range * range;

        for (int i = 0; i < hits.Length; i++)
        {
            var h = hits[i];
            if (!h || !h.CompareTag("Enemy")) continue;

            Vector3 to = h.transform.position - transform.position;
            to.y = 0f;

            if (to.sqrMagnitude > rangeSqr) continue;
            float ang = Vector3.Angle(fwd, to);
            if (ang > halfAngle) continue;

            var health = h.GetComponentInParent<Health>();
            if (health != null) health.TakeDamage(damage);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.forward * forwardOffset, sphereRadius);
    }
}
