using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UnityEvent onPowerUpCollected;
    [SerializeField] private GameConfig config;
    public GameConfig Config => config;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    public void RaisePowerUpCollected() => onPowerUpCollected?.Invoke();
}
