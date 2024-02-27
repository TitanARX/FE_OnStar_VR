using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightModel : MonoBehaviour
{
    public Transform AnchorTarget => Camera.main.transform;

    [SerializeField]
    private float _trackedHeadPosition = 1.01f;

    public float TrackedHeadPosition { get => _trackedHeadPosition; set => _trackedHeadPosition = value; }

    [SerializeField]
    [Range(0.01f, 0.5f)]
    private float offset = 0.1f;

    private void Update()
    {
        TrackedHeadPosition = AnchorTarget ? AnchorTarget.position.y - offset : 1f;
    }


}
