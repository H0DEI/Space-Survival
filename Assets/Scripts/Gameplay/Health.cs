using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class DamagedEvent : UnityEvent<int, int> { } // current, max

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 5;
    private int currentHP;

    public UnityEvent OnDeath;
    public DamagedEvent OnDamaged;  // NEW

    public int Current => currentHP;
    public int Max => maxHP;

    void Awake() => currentHP = maxHP;

    public void TakeDamage(int amount)
    {
        if (currentHP <= 0) return;
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        OnDamaged?.Invoke(currentHP, maxHP);  // notify UI

        if (currentHP <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnDamaged?.Invoke(currentHP, maxHP);
    }
}
