using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public enum VariableType
{
    Bool,
    Vect2,
    Vect3,
    Float,
    Integer,
    Color
}

[System.Serializable]
public class BoolDataTarget
{
    public string stateName = "";
    public bool targetValue;
    public float duration = 0.3f;
}

[System.Serializable]
public class Vect2DataTarget
{
    public string stateName = "";
    public Vector2 targetValue;
    public float duration = 0.3f;

}

[System.Serializable]
public class Vect3DataTarget
{
    public string stateName = "";
    public Vector3 targetValue;
    public float duration = 0.3f;

}

[System.Serializable]
public class FloatDataTarget
{
    public string stateName = "";
    public float targetValue;
    public float duration = 0.3f;

}

[System.Serializable]
public class IntegerDataTarget
{
    public string stateName = "";
    public int targetValue;
    public float duration = 0.3f;

}

[System.Serializable]
public class ColorDataTarget
{
    public string stateName = "";
    public Color targetValue;
    public float duration = 0.3f;

}

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

[System.Serializable]
public class Vect2Event : UnityEvent<Vector2> { }

[System.Serializable]
public class Vect3Event : UnityEvent<Vector3> { }

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

[System.Serializable]
public class IntegerEvent : UnityEvent<int> { }

[System.Serializable]
public class ColorEvent : UnityEvent<Color> { }


public class ARXStatemachineVariable : MonoBehaviour
{
    public ResponderModel model = new ResponderModel();

    private string _currentState;

    [Header("Dynamic Variables")]
    [Space]
    public VariableType selectedVariableType;

    public BoolEvent _boolEvent;
    public Vect2Event _vect2Event;
    public Vect3Event _vect3Event;
    public FloatEvent _floatEvent;
    public IntegerEvent _intEvent;
    public ColorEvent _colorEvent;

    [Header("Dynamic Target Variables")]
    [Space]
    public List<BoolDataTarget> BoolDataTargets = new List<BoolDataTarget>();
    public List<Vect2DataTarget> Vect2DataTargets = new List<Vect2DataTarget>();
    public List<Vect3DataTarget> Vect3DataTargets = new List<Vect3DataTarget>();
    public List<FloatDataTarget> FloatDataTargets = new List<FloatDataTarget>();
    public List<IntegerDataTarget> IntDataTargets = new List<IntegerDataTarget>();
    public List<ColorDataTarget> ColorDataTargets = new List<ColorDataTarget>();



    private void OnValidate()
    {
        if (model == null)
        {
            Debug.Log("Model is null in " + this.name);
            return;
        }

        if (model.StateMachineReference == null)
        {
            Debug.Log("Missing Statemachine in " + this.name);
        }
        else if (model.StateMachineReference.stateDefinitions == null)
        {
            Debug.Log("Missing State Definitions in " + this.name);
        }
        else
        {
            model.States = model.StateMachineReference.stateDefinitions.States;
            Debug.Log(this.transform.name + " Listening for the state " + model.SelectedState);
            PopulateDataTargets();
        }
    }

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        if (model.StateMachineReference != null)
        {
            model.StateMachineReference.OnSetStateVariable.AddListener(HandleBeginTransitionToState);

        }
    }

    private void OnDestroy()
    {
        if (model.StateMachineReference != null)
        {
            // Remove only the DebugOutput listener
            model.StateMachineReference.OnSetStateVariable.RemoveListener(HandleBeginTransitionToState);

            _boolEvent.AddListener(TweenBoolDataTarget);
        }
    }

    private void OnDisable()
    {
        model.StateMachineReference.BeginTransitionToState.RemoveListener(HandleBeginTransitionToState);

        _boolEvent.RemoveListener(TweenBoolDataTarget);
    }

    private void PopulateDataTargets()
    {
        // Ensure each DataTarget list has the same number of elements as the number of states
        int numStates = model.States.Count;

        if (numStates == 0)
            return;

        EnsureListCount(BoolDataTargets, numStates);
        EnsureListCount(Vect2DataTargets, numStates);
        EnsureListCount(Vect3DataTargets, numStates);
        EnsureListCount(FloatDataTargets, numStates);
        EnsureListCount(IntDataTargets, numStates);
        EnsureListCount(ColorDataTargets, numStates);

        // Set stateName in each DataTarget to match the corresponding state name
        for (int i = 0; i < numStates; i++)
        {
            string stateName = model.States[i];
            if (i < BoolDataTargets.Count)
                BoolDataTargets[i].stateName = stateName;
            if (i < Vect2DataTargets.Count)
                Vect2DataTargets[i].stateName = stateName;
            if (i < Vect3DataTargets.Count)
                Vect3DataTargets[i].stateName = stateName;
            if (i < FloatDataTargets.Count)
                FloatDataTargets[i].stateName = stateName;
            if (i < IntDataTargets.Count)
                IntDataTargets[i].stateName = stateName;
            if (i < ColorDataTargets.Count)
                ColorDataTargets[i].stateName = stateName;
        }
    }

    private void EnsureListCount<T>(List<T> list, int count)
    {
        while (list.Count < count)
        {
            list.Add(default(T));
        }
        while (list.Count > count)
        {
            list.RemoveAt(list.Count - 1);
        }
    }


    private void HandleBeginTransitionToState(string state)
    {
        _currentState = state;

        // Retrieve the corresponding DataTarget for the current state
        switch (selectedVariableType)
        {
            case VariableType.Bool:

                BoolDataTarget dataTarget = BoolDataTargets.Find(target => target.stateName == _currentState);

                // Invoke the BoolEvent
                _boolEvent.Invoke(dataTarget.targetValue);

                break;
            case VariableType.Vect2:

                TweenVect2DataTarget();

                break;
            case VariableType.Vect3:
                // Handle Vect3 case
                break;
            case VariableType.Float:
                // Handle Float case
                break;
            case VariableType.Integer:
                // Handle Integer case
                break;
            case VariableType.Color:
                // Handle Color case
                break;
            default:
                Debug.LogError("Unsupported VariableType: " + selectedVariableType);
                break;
        }

    }

    private void TweenBoolDataTarget(bool _value)
    {
        Debug.Log(_boolEvent);

        BoolDataTarget dataTarget = BoolDataTargets.Find(target => target.stateName == _currentState);

        if (dataTarget != null)
        {
            bool targetValue = dataTarget.targetValue;

            float duration = dataTarget.duration;
        }
    }


    private void TweenVect2DataTarget()
    {     
        Vect2DataTarget dataTarget = Vect2DataTargets.Find(target => target.stateName == _currentState);

        if (dataTarget != null)
        {
            Vector2 targetValue = dataTarget.targetValue;

            float duration = dataTarget.duration;

            Vector2 defaultValue = Vector2.zero;

            DOTween.To(() => defaultValue, x => defaultValue = x, targetValue, duration).OnUpdate( delegate { _vect2Event?.Invoke(defaultValue); });
        }
        
    }

    private void TweenVect3DataTarget()
    {
        /*
        Vect3DataTarget dataTarget = Vect3DataTargets.Find(target => target.stateName == state);
        if (dataTarget != null)
        {
            Vector3 targetValue = dataTarget.targetValue;
            float duration = dataTarget.duration;

            //DOTween.To(() => dynamicVariable.vect3Value, x => dynamicVariable.vect3Value = x, targetValue, duration);
        }
        */
    }

    private void TweenFloatDataTarget()
    {
        /*
        FloatDataTarget dataTarget = FloatDataTargets.Find(target => target.stateName == state);

        if (dataTarget != null)
        {
            float targetValue = dataTarget.targetValue;
            float duration = dataTarget.duration;

            //DOTween.To(() => dynamicVariable.floatValue, x => dynamicVariable.floatValue = x, targetValue, duration);
        }
        */
    }

    private void TweenIntegerDataTarget()
    {
        /*
        IntegerDataTarget dataTarget = IntDataTargets.Find(target => target.stateName == state);
        if (dataTarget != null)
        {
            int targetValue = dataTarget.targetValue;
            float duration = dataTarget.duration;

            //DOTween.To(() => dynamicVariable.intValue, x => dynamicVariable.intValue = x, targetValue, duration);
        }
        */
    }

    private void TweenColorDataTarget()
    {
        /*
        ColorDataTarget dataTarget = ColorDataTargets.Find(target => target.stateName == state);
        if (dataTarget != null)
        {
            Color targetValue = dataTarget.targetValue;
            float duration = dataTarget.duration;

            //DOTween.To(() => dynamicVariable.colorValue, x => dynamicVariable.colorValue = x, targetValue, duration);
        }
        */
    }
}


