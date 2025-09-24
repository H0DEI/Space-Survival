using UnityEngine;
using UnityEngine.Events;

public class EventTimingDemo : MonoBehaviour
{
    public UnityEvent myEvent;

    private void Start()
    {
        Debug.Log("[EventTimingDemo] Invoke in Start");
        myEvent.Invoke(); // Now listeners registered in Start are already active
    }

}