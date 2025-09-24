using UnityEngine;
using System.Collections;

public class AutoAimAtEnemy : MonoBehaviour
{
    [Header("Objeto a rotar (hijo visual)")]
    [SerializeField] private Transform modelToRotate;

    [Header("Detección")]
    [SerializeField] private float searchRadius = 30f;
    [SerializeField] private LayerMask enemyMask = ~0;

    [Header("Ajustes de apuntado")]
    [SerializeField] private float preAimTime = 0.5f;
    [SerializeField] private float postAimTime = 0.5f;
    [SerializeField] private float rotationSpeed = 12f;

    [Tooltip("Corrige si tu modelo no mira hacia +Z en reposo (ej: pon 90 si mira hacia +X)")]
    [SerializeField] private float headingOffsetDeg = 0f;

    private Transform currentTarget;
    private bool isAiming;
    public bool IsAiming => isAiming;

    private readonly Collider[] overlapBuf = new Collider[64];
    

    public void TriggerAim()
    {
        if (isAiming) return;

        currentTarget = FindClosestEnemy();
        if (currentTarget != null)
            StartCoroutine(AimWindow());
    }

    private IEnumerator AimWindow()
    {
        isAiming = true;
        yield return new WaitForSeconds(preAimTime);
        yield return new WaitForSeconds(postAimTime);
        isAiming = false;
        currentTarget = null;
    }

    void Update()
    {
        if (!isAiming || currentTarget == null || modelToRotate == null) return;

        Vector3 to = currentTarget.position - transform.position;
        to.y = 0f;

        if (to.sqrMagnitude < 0.0001f) return;

        float angleDeg = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg + headingOffsetDeg;

        Quaternion targetRot = Quaternion.Euler(0f, angleDeg, 0f);

        modelToRotate.rotation = Quaternion.Slerp(
            modelToRotate.rotation,
            targetRot,
            Time.deltaTime * rotationSpeed
        );
    }

    private Transform FindClosestEnemy()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, overlapBuf, enemyMask);
        float best = float.PositiveInfinity; Transform bestT = null;

        for (int i = 0; i < count; i++)
        {
            var col = overlapBuf[i];
            if (!col) continue;

            var h = col.GetComponentInParent<Health>();
            if (h == null) continue;

            float d = (h.transform.position - transform.position).sqrMagnitude;
            if (d < best) { best = d; bestT = h.transform; }
        }
        return bestT;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
