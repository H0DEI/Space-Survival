using UnityEngine;

public class EventListenerDemo : MonoBehaviour
{
    private void Start()
    {
        var invoker = GetComponent<EventTimingDemo>();
        invoker.myEvent.AddListener(() => Debug.Log("[EventListenerDemo] Reacted!"));
        Debug.Log("[EventListenerDemo] Listener added in Start");
    }
}
