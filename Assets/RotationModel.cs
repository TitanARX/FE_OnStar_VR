using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationModel : MonoBehaviour
{
    public Transform AnchorTarget => Camera.main.transform;

    [SerializeField]
    private Vector3 _trackedRotation;

    public Vector3 TrackedRotation { get => _trackedRotation; set => _trackedRotation = value; }


    // Update is called once per frame
    void Update()
    {
        TrackedRotation = AnchorTarget.localEulerAngles;        
    }
}
