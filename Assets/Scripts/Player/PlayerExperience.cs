using UnityEngine;
using UnityEngine.Events;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int startLevel = 1;
    [SerializeField] private int baseXpToNext = 5;
    [SerializeField] private int xpPerLevel = 5;
    [SerializeField] private int maxLevel = 50;

    public UnityEvent onLevelUp;

    public int CurrentLevel => level;
    public int CurrentXP => currentXP;
    public int XPToNext => xpToNext;

    private int level;
    private int currentXP;
    private int xpToNext;
    private AttackManager attackManager;

    void Awake()
    {
        level = Mathf.Max(1, startLevel);
        xpToNext = baseXpToNext;
        attackManager = GetComponent<AttackManager>();
    }

    public void AddExp(int amount)
    {
        if (level >= maxLevel) return;

        currentXP += Mathf.Max(0, amount);
        while (currentXP >= xpToNext && level < maxLevel)
        {
            currentXP -= xpToNext;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNext += xpPerLevel;

        onLevelUp?.Invoke();

        if (attackManager != null)
            attackManager.UnlockNext();
    }
}
