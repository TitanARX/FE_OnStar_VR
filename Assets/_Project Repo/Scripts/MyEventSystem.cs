using UnityEngine;
using System;
using System.Collections.Generic;

public class MyEventSystem<T> : MonoBehaviour
{
    // Reference to StateManager
    public StateManager stateManager;

    // Define a list to hold listener-parameter pairs
    public List<ListenerParameterPair<T>> listenerParameterPairs = new List<ListenerParameterPair<T>>();

    // Invoke the event, invoking each listener with its associated parameter
    public void InvokeEvent()
    {
        foreach (var pair in listenerParameterPairs)
        {
            // Ensure the target object and method name are valid
            if (pair.targetObject != null && !string.IsNullOrEmpty(pair.methodName))
            {
                // Find the method by name and invoke it on the target object
                var method = pair.targetObject.GetType().GetMethod(pair.methodName);
                if (method != null)
                {
                    method.Invoke(pair.targetObject, new object[] { pair.parameter });
                }
                else
                {
                    Debug.LogError($"Method {pair.methodName} not found on {pair.targetObject.name}");
                }
            }
            else
            {
                Debug.LogError("Invalid listener configuration");
            }
        }
    }
}
