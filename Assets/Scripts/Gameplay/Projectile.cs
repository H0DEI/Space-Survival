using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;
    private Vector3 _dir;

    public void Launch(Vector3 dir)
    {
        _dir = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += _dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<Health>()?.TakeDamage(damage);
            Debug.Log("Projectile hit: " + other.name);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }
}
