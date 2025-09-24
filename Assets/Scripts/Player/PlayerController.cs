using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.Serializable] public struct Velocity { public float x, z; } // VALUE
    private class RuntimeState { public int collected; }                // REF

    [SerializeField] private Rigidbody rb;

    [Header("Visual rotation")]
    [SerializeField] private Transform modelToRotate;   // child model to rotate; if null, uses this.transform
    [SerializeField] private float moveTurnSpeed = 12f; // movement-facing turn speed

    private RuntimeState state = new RuntimeState();
    private Velocity vel;

    // Cached systems
    private AutoAimAtEnemy aimer;

    // Input & movement
    private Vector3 inputDir;     // normalized input on XZ
    private Vector3 desiredVel;   // desired velocity on XZ

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!modelToRotate) modelToRotate = transform;
        aimer = GetComponent<AutoAimAtEnemy>(); // optional
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Normalize input to avoid diagonal speed boost
        Vector3 raw = new Vector3(h, 0f, v);
        inputDir = raw.sqrMagnitude > 1f ? raw.normalized : raw; // keep analog magnitude but cap to 1

        float speed = GameManager.Instance.Config.moveSpeed;
        desiredVel = inputDir * speed;

        // Rotate towards movement if not aiming
        bool isAiming = (aimer != null && aimer.IsAiming);
        if (!isAiming && inputDir.sqrMagnitude > 0.0001f)
        {
            // Y-up: face movement on XZ plane
            float angleDeg = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0f, angleDeg, 0f);

            modelToRotate.rotation = Quaternion.Slerp(
                modelToRotate.rotation,
                targetRot,
                Time.deltaTime * moveTurnSpeed
            );
        }
        // else: aim script controls rotation
    }

    void FixedUpdate()
    {
        // Apply velocity in physics step (keeps motion consistent)
        rb.velocity = new Vector3(desiredVel.x, 0f, desiredVel.z);
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
