using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField] private int dps = 1;            // damage per second
    [SerializeField] private float tick = 0.5f;      // how often we apply damage
    private float _t;

    void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        _t += Time.deltaTime;
        if (_t >= tick)
        {
            _t = 0f;
            collision.collider.GetComponent<Health>()?.TakeDamage(Mathf.RoundToInt(dps * tick));
        }
    }
}
