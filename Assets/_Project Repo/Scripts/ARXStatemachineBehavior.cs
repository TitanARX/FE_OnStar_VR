using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ARXStatemachineBehavior : MonoBehaviour
{   
    public ARXStateEnumAsset stateDefinitions; // Reference to the ARXStateEnumAsset

    public bool _startOnAwake = false;

    [SerializeField]
    private string defaultState = "";

    [SerializeField]
    private string currentState = "";

    [SerializeField]
    private string previousState = "";

    public string CurrentState { get => currentState; }

    public string PreviousState { get => previousState; }

    private float delay => stateDefinitions._delay;


    [Header("Initialization Event")]
    public UnityEvent OnInitStateMachine;

    [Header("Global State Change Events")]
    public UnityEvent<string> BeginTransitionToState;
    public UnityEvent<string> OnFinishTransitionToState;
    public UnityEvent<string> OnBeginTransitionFromState;
    public UnityEvent<string> OnFinishTransitionFromState;

    public UnityEvent<string> OnSetStateVariable;

    private void OnValidate()
    {
        if (stateDefinitions.States.Count == 0)
        {
            Debug.LogWarning("ARXStatemachineBehavior: States list is empty.");
        }
        else
        {
            defaultState = stateDefinitions.States.Count > 0 && _startOnAwake ? stateDefinitions.States[0] : "";
        }
    }

    private void Awake()
    {
        if (_startOnAwake == false)
            return;

        OnInit();
    }

    public void OnInit()
    {
        OnInitStateMachine?.Invoke();

        TransitionToState(defaultState);
    }

    public void TransitionToState(string state)
    {
        if(stateDefinitions.States.Contains(state) == false)
        {
            Debug.Log("The target state " + state + " does not exist. Verify state name exist in definitions SO");
        }
        else
        {
            if (currentState == state)
            {
                Debug.Log("The target state " + state + " is already the current state");
            }
            else
            {
                StartCoroutine(Transition(state));
            }
        }
    }

    IEnumerator Transition(string state)
    {
        OnSetStateVariable?.Invoke(state);

        if (currentState != string.Empty || state != currentState)
        {
            previousState = currentState;

            OnBeginTransitionFromState?.Invoke(state);

            yield return new WaitForSeconds(0.01f);

            OnFinishTransitionFromState?.Invoke(state);

            currentState = state;

            Debug.Log("Beginning Transition");

            BeginTransitionToState?.Invoke(state);

            yield return new WaitForSeconds(0.01f);

            OnFinishTransitionToState?.Invoke(state);
        }
    }

    public void TransitionToStateDelayed(string state)
    {
        StartCoroutine(TransitionDelay(state));
    }

    IEnumerator TransitionDelay(string state)
    {
        OnSetStateVariable?.Invoke(state);

        yield return new WaitForSeconds(delay);

        if (currentState != null || state != currentState)
        {
            OnBeginTransitionFromState?.Invoke(currentState);
            
            currentState = state;
            
            BeginTransitionToState?.Invoke(state);

            OnBeginTransitionFromState?.Invoke(state);

            yield return null;
            
            OnFinishTransitionToState?.Invoke(state);
        }     
    }



}
