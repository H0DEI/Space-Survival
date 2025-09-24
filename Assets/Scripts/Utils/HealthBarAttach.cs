using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthBarAttach : MonoBehaviour
{
    [SerializeField] private HealthBarUI healthBarPrefab;
    [SerializeField] private Transform uiRoot;           
    [SerializeField] private float offsetZ = 0.7f;       

    private HealthBarUI instance;
    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        if (instance == null && healthBarPrefab != null)
        {
            Transform parent = uiRoot != null ? uiRoot : null;
            instance = Instantiate(healthBarPrefab, parent);
            instance.name = $"{gameObject.name}_HealthBar";

            instance.Initialize(health, new Vector3(0f, 0f, offsetZ));
        }
    }

    void OnDisable()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}
