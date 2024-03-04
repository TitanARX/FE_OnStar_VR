using UnityEngine;
using UnityEngine.Events;

public class HeadsetTrigger : MonoBehaviour
{
    public UnityEvent OnUseHeadset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Headset"))
        {
            // Destroy the headset object
            Destroy(other.gameObject);

            // Invoke the UnityEvent
            if (OnUseHeadset != null)
            {
                OnUseHeadset.Invoke();
            }
        }
    }
}
