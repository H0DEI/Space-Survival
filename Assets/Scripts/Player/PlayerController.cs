using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.Serializable] public struct Velocity { public float x, z; } // VALUE
    private class RuntimeState { public int collected; }                // REF

    [SerializeField] private Rigidbody rb;
    private RuntimeState state = new RuntimeState();
    private Velocity vel;

    void Awake() { if (!rb) rb = GetComponent<Rigidbody>(); }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float speed = GameManager.Instance.Config.moveSpeed;

        vel.x = h * speed;
        vel.z = v * speed;

        var dir = new Vector3(vel.x, 0f, vel.z);
        rb.velocity = new Vector3(dir.x, 0f, dir.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            state.collected++;
            GameManager.Instance.RaisePowerUpCollected();
            Destroy(other.gameObject);
        }
    }
}
