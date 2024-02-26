using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ARXStateEnumAsset", menuName = "Custom/ARX State Enum Asset")]
public class ARXStateEnumAsset : ScriptableObject
{
    [Header("Initial State Delay")]
    public float _delay;

    [SerializeField]
    private List<string> _states = new List<string>();

    public List<string> States
    {
        get { return _states; }
    }
}

