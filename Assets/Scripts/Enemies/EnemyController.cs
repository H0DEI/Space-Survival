using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageTick = 0.5f;

    private Transform player;
    private Rigidbody rb;
    private float tick;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (!player) return;

        Vector3 dir = (player.position - rb.position);
        dir.y = 0f;
        dir = dir.normalized;

        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        tick += Time.deltaTime;
        if (tick >= damageTick)
        {
            tick = 0f;
            collision.collider.GetComponent<Health>()?.TakeDamage(contactDamage);
        }
    }
}
