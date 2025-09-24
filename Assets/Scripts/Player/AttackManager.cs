using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Tooltip("Attacks that will be unlocked in order")]
    [SerializeField] private MonoBehaviour[] unlockableAttacks;

    private int nextIndex;

    void Awake()
    {
        // Disable all unlockable attacks at start
        for (int i = 0; i < unlockableAttacks.Length; i++)
        {
            if (unlockableAttacks[i] != null)
                unlockableAttacks[i].enabled = false;
        }
        nextIndex = 0;
    }

    public void UnlockNext()
    {
        if (nextIndex >= unlockableAttacks.Length) return;
        if (unlockableAttacks[nextIndex] != null)
            unlockableAttacks[nextIndex].enabled = true;

        nextIndex++;
    }
}
