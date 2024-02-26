using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResponderModel
{    
   
    [SerializeField]
    private ARXStatemachineBehavior _statemachine = null; // Reference to the ARXStatemachineBehavior

    public ARXStatemachineBehavior StateMachineReference
    {
        get { return _statemachine; }
        set { _statemachine = value; }
    }

    [SerializeField]
    private string _selectedState = "";

    public string SelectedState
    {
        get { return _selectedState; }
        set { _selectedState = value; }
    }

    public string PreviousState { get => StateMachineReference.PreviousState; }

    public List<string> States = new List<string>();

}

public class ARXStatemachineResponder : MonoBehaviour
{


    public ResponderModel model = new ResponderModel();

    [Header("Responder State Events")]
    [Space]
    public UnityEvent<string> R_BeginTransitionToState;
    public UnityEvent<string> R_OnFinishTransitionToState;
    public UnityEvent<string> R_OnBeginTransitionFromState;
    public UnityEvent<string> R_OnFinishTransitionFromState;


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
        }
    }



    private void Awake()
    {
        if (model.StateMachineReference != null)
        {
            model.StateMachineReference.BeginTransitionToState.AddListener(HandleBeginTransitionToState);
            model.StateMachineReference.OnFinishTransitionToState.AddListener(HandleFinishTransitionToState);
            model.StateMachineReference.OnBeginTransitionFromState.AddListener(HandleBeginTransitionFromState);
            model.StateMachineReference.OnFinishTransitionFromState.AddListener(HandleFinishTransitionFromState);
        }
    }

    private void OnDestroy()
    {
        if (model.StateMachineReference != null)
        {
            model.StateMachineReference.BeginTransitionToState.RemoveListener(HandleBeginTransitionToState);
            model.StateMachineReference.OnFinishTransitionToState.RemoveListener(HandleFinishTransitionToState);
            model.StateMachineReference.OnBeginTransitionFromState.RemoveListener(HandleBeginTransitionFromState);
            model.StateMachineReference.OnFinishTransitionFromState.RemoveListener(HandleFinishTransitionFromState);
        }
    }

    private void HandleBeginTransitionToState(string state)
    {
        if (model.SelectedState != state)
            return;

        // Handle BeginTransitionToState event for ARXStatemachineResponder
        R_BeginTransitionToState?.Invoke(state);
    }

    private void HandleFinishTransitionToState(string state)
    {
        Debug.Log(model.SelectedState + " / " + state);

        if (model.SelectedState != state)
            return;

        // Handle OnFinishTransitionToState event for ARXStatemachineResponder
        R_OnFinishTransitionToState?.Invoke(state);
    }

    private void HandleBeginTransitionFromState(string state)
    {
        if (model.SelectedState != model.PreviousState)
            return;

        // Handle OnBeginTransitionFromState event for ARXStatemachineResponder
        R_OnBeginTransitionFromState?.Invoke(state);
    }

    private void HandleFinishTransitionFromState(string state)
    {
        if (model.SelectedState != model.PreviousState)
            return;

        // Handle OnFinishTransitionFromState event for ARXStatemachineResponder
        R_OnFinishTransitionFromState?.Invoke(state);
    }



}
