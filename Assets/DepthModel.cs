using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthModel : MonoBehaviour
{
    [SerializeField]
    [Range(0.3f, 0.5f)]
    private float _trackedDepth = 0.498f;

    public float TrackedDepth { get => _trackedDepth; set => _trackedDepth = value; }

}
