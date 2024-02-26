using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Reference to the event system
    public MyEventSystem<bool> eventSystem;

    public List<ListenerParameterPair<bool>> listenerParameterPairs = new List<ListenerParameterPair<bool>>();


    // Method to invoke events
    public void InvokeMyEvent(bool parameter)
    {
        // Add any necessary logic here

        // Invoke the event
        eventSystem.InvokeEvent();
    }
}
