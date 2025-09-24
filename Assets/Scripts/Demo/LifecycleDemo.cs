using UnityEngine;

public class LifecycleDemo : MonoBehaviour
{
    private void Awake() { Debug.Log("[LifecycleDemo] Awake"); }
    private void OnEnable() { Debug.Log("[LifecycleDemo] OnEnable"); }
    private void Start() { Debug.Log("[LifecycleDemo] Start"); }
    private void OnDisable() { Debug.Log("[LifecycleDemo] OnDisable"); }
    private void OnDestroy() { Debug.Log("[LifecycleDemo] OnDestroy"); }
}
