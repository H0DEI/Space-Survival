using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private int expValue = 1;

    [Header("Magnet")]
    [SerializeField] private float magnetRadius = 6f;
    [SerializeField] private float startSpeed = 4f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float accel = 40f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 30f;

    private Transform player;
    private bool attracted;
    private float curSpeed;

    void Awake()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        curSpeed = startSpeed;
        if (lifeTime > 0f) Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (!attracted && dist <= magnetRadius)
            attracted = true;

        if (attracted)
        {
            curSpeed = Mathf.Min(maxSpeed, curSpeed + accel * Time.deltaTime);
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * curSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var xp = other.GetComponent<PlayerExperience>();
        if (xp == null) xp = other.GetComponentInParent<PlayerExperience>();
        if (xp != null)
            xp.AddExp(expValue);

        Destroy(gameObject);
    }
}
