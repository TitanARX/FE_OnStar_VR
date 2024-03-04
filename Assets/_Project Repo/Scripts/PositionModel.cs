using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PositionModel : MonoBehaviour
{
    public Transform AnchorTarget => Camera.main.transform;

    [SerializeField]
    private Vector3 _trackedPosition;

    public Vector3 TrackedPosition { get => _trackedPosition; set => _trackedPosition = value; }

    [Range(0.1f, 10f)]
    private float trackingSpeed = 0.1f;


    private void Update()
    {
        TrackedPosition = AnchorTarget ? new Vector3(AnchorTarget.position.x, 0, AnchorTarget.position.z) : new Vector3(0, 1f, 0.498f);
    }





}
