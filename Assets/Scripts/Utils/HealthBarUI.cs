using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fill;                
    [SerializeField] private CanvasGroup cg;            
    [SerializeField] private bool alwaysVisible = false;
    [SerializeField] private bool billboard = true;

    [SerializeField] private Vector3 offset = new Vector3(0f, 1.6f, 0f);

    private Health health;
    private bool hasBeenHit = false;

    public void Initialize(Health target, Vector3 worldOffset)
    {
        health = target;
        offset = worldOffset;

        if (!cg) cg = GetComponent<CanvasGroup>();
        if (cg) cg.alpha = alwaysVisible ? 1f : 0f;

        Subscribe();
        UpdateFillImmediate();
    }

    void Awake()
    {
        if (!cg) cg = GetComponent<CanvasGroup>();
        if (cg && !alwaysVisible) cg.alpha = 0f;
    }

    void OnEnable() { Subscribe(); }
    void OnDisable() { Unsubscribe(); }

    private void Subscribe()
    {
        if (health == null) return;
        health.OnDamaged.RemoveListener(OnDamaged);
        health.OnDeath.RemoveListener(OnDeath);
        health.OnDamaged.AddListener(OnDamaged);
        health.OnDeath.AddListener(OnDeath);
    }

    private void Unsubscribe()
    {
        if (health == null) return;
        health.OnDamaged.RemoveListener(OnDamaged);
        health.OnDeath.RemoveListener(OnDeath);
    }

    void LateUpdate()
    {
        if (health == null) return;

        transform.position = health.transform.position + offset;

        if (billboard && Camera.main)
        {
            transform.rotation = Quaternion.LookRotation(
                Camera.main.transform.forward,
                Vector3.up
            );
        }
    }

    private void OnDamaged(int current, int max)
    {
        if (fill) fill.fillAmount = max > 0 ? (float)current / max : 0f;

        if (cg && !alwaysVisible)
        {
            cg.alpha = 1f; 
            if (!hasBeenHit) hasBeenHit = true;
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    private void UpdateFillImmediate()
    {
        if (health != null && fill)
            fill.fillAmount = health.Max > 0 ? (float)health.Current / health.Max : 0f;
    }
}
