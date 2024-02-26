using UnityEngine;
using System;

[Serializable]
public class ListenerParameterPair<T>
{
    public T parameter;
    public UnityEngine.Object targetObject;
    public string methodName;
}
