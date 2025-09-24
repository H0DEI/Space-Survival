using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIObserver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;

    private void OnEnable()
    {
        // Safe subscribe
        StartCoroutine(WaitAndSubscribe());
    }

    private IEnumerator WaitAndSubscribe()
    {
        // Wait until the GameManager singleton is ready
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.onPowerUpCollected.AddListener(ShowMsg);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.onPowerUpCollected.RemoveListener(ShowMsg);
    }

    private void ShowMsg()
    {
        if (message) message.text = "Power–Up collected!";
        Debug.Log("The EventObserver is reacting to the Event");
    }
}
