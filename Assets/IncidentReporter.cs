using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IncidentReporter : MonoBehaviour
{
    public int targetIncident = 1;

    public Dropdown dropdown;

    public UnityEvent OnSelectTargetIncident;

    private void Start()
    {
        // Subscribe to the onValueChanged event to detect changes in the dropdown selection
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    void DropdownValueChanged(Dropdown dropdown)
    {
        // Get the current selection index
        int selectedIndex = dropdown.value;

        // Get the current selection text
        string selectedText = dropdown.options[selectedIndex].text;

        // Output the current selection
        Debug.Log("Selected index: " + selectedIndex + ", Selected text: " + selectedText);

        if(selectedIndex == targetIncident)
        {
            OnSelectTargetIncident?.Invoke();
        }


    }
}
