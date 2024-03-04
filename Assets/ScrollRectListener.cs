using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrollRectListener : MonoBehaviour
{
    public ScrollRect scrollRect; // Reference to the ScrollRect component
    public float targetScrollValue = 0.5f; // Target scroll value to trigger the event
    public UnityEvent onVehicleFound; // UnityEvent to call when the target scroll value is reached

    private bool eventFired = false; // Flag to ensure event is fired only once

    private void Update()
    {
        // Check if the current scroll value matches the target scroll value
        if (scrollRect.verticalNormalizedPosition == targetScrollValue && !eventFired)
        {
            // Call the UnityEvent
            onVehicleFound.Invoke();
            eventFired = true; // Set the flag to true to ensure the event is fired only once
        }
    }
}

