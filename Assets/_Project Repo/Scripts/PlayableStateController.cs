using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.Collections;

public enum Status { Unitialized, Initialized}
public enum PlayStatus { Stopped, Playing, Paused }


[System.Serializable]
public enum ProgressionState { None, Forward, Direct }

[System.Serializable]
public class PlayableDirectorModel
{
    public string indexName;
    public DirectorWrapMode currentWrapMode;
    public ProgressionState progressionState = ProgressionState.None;
    public int targetIndex;
    public PlayableAsset timeline;
    public PlayableDirectorEvent directorEvent;
    public float transitionDuration = 0.1f;
}

[System.Serializable]
public class PlayableDirectorEvent
{
    public UnityEvent OnInit;
    public UnityEvent OnPlay;
    public UnityEvent OnResume;
    public UnityEvent OnPause;
    public UnityEvent OnFinish; // Custom event for handling the end of the timeline.
}

[RequireComponent(typeof(PlayableDirector))]
public class PlayableStateController : MonoBehaviour
{
    public Status status = Status.Unitialized;
    public PlayStatus playStatus = PlayStatus.Stopped;

    
    public PlayableDirector Director;
    public List<PlayableDirectorModel> DirectorModels = new List<PlayableDirectorModel>();
    private int currentIndex = 0;

    private Coroutine TimelinePositionCheck;
    public float progressionDelay;

    private void Awake()
    {
        status = Status.Unitialized;

        InitializeStateSequener();
    }

    private void OnEnable()
    {
        Director.played += OnTimelineStarted;
        Director.stopped += OnTimelineStopped;
    }

    private void OnDisable()
    {
        Director.played -= OnTimelineStarted;
        Director.stopped -= OnTimelineStopped;
    }

    private void OnValidate()
    {
        Director = GetComponent<PlayableDirector>();

        AssignIndexNames();
    }


    public void InitializeStateSequener()
    {
        if (Director == null)
            return;

        
        Director.extrapolationMode = DirectorWrapMode.None;
        Director.playOnAwake = false;

        foreach (var evt in DirectorModels)
        {
            evt.directorEvent.OnPlay.AddListener(UpdateStateMachine);
        }

        status = Status.Initialized;

    }

    private void UpdateStateMachine()
    {
       //Implement the Statemachine
    }

    private void AssignIndexNames()
    {
        foreach (var item in DirectorModels)
        {
            if (item.timeline == null)
                return;

            item.indexName = item.timeline.name;
            
        }
    }

    #region Helper Methods

    public int GetStateIndexFromName(string stateName)
    {
        var index = 0;

        foreach (var state in DirectorModels)
        {
            if(state.indexName.Equals(stateName))
            {
                index = DirectorModels.IndexOf(state);
            }
        }

        return index;
    }

    public string GetStateNameFromIndex(int stateIndex)
    {
        return DirectorModels[stateIndex].indexName;
    }


    public string GetNextStateName()
    {
        var state = string.Empty;

        var currentDirectorModel = DirectorModels[currentIndex];

        if(currentDirectorModel.currentWrapMode == DirectorWrapMode.Loop)
        {
            state = DirectorModels[currentIndex].indexName;
        }
        else
        {
            switch (currentDirectorModel.progressionState)
            {
                case ProgressionState.None:

                    state = DirectorModels[currentIndex].indexName;

                    break;
                case ProgressionState.Forward:

                    int increment = 1;

                    string proceedingState = DirectorModels[(currentIndex + increment) % DirectorModels.Count].indexName;

                    state = proceedingState;

                    break;
                case ProgressionState.Direct:

                    state = DirectorModels[currentDirectorModel.targetIndex].indexName;

                    break;
                default:
                    break;
            }
        }

        return state;
    }

    public int GetNextStateIndex()
    {
        var state = 0;

        if(DirectorModels.Count.Equals(0))
        {
            state = 0;
        }
        else
        {
            var currentDirectorModel = DirectorModels[currentIndex];

            if (currentDirectorModel.currentWrapMode == DirectorWrapMode.Loop)
            {
                state = DirectorModels.IndexOf(currentDirectorModel);
            }
            else
            {
                switch (currentDirectorModel.progressionState)
                {
                    case ProgressionState.None:

                        state = currentIndex;

                        break;
                    case ProgressionState.Forward:

                        int increment = 1;

                        int proceedingSequence = (currentIndex + increment) % DirectorModels.Count;

                        state = proceedingSequence;

                        break;
                    case ProgressionState.Direct:

                        state = currentDirectorModel.targetIndex;

                        break;
                    default:
                        break;
                }
            }
        }

        return state;
    }


    public int GetPreviousStateIndex()
    {
        //TODO
        return 0;
    }


    #endregion


    private void OnTimelineStarted(PlayableDirector playableDirector)
    {
        if (TimelinePositionCheck != null)
        {
            StopCoroutine(TimelinePositionCheck);
        }


        TimelinePositionCheck = StartCoroutine(CheckTimelineEnd());
    }

    private void OnTimelineStopped(PlayableDirector playableDirector)
    {
        if (TimelinePositionCheck != null)
            StopCoroutine(TimelinePositionCheck);
    }


    //Called When Playing
    public IEnumerator CheckTimelineEnd()
    {
        while(!Mathf.Approximately((float)Director.time, (float)Director.duration))
        {
            yield return null;
        }

        Debug.Log("End of Sequence Detected");

        DirectorModels[currentIndex].directorEvent.OnFinish?.Invoke();

        yield return new WaitForSeconds(progressionDelay);

        switch (DirectorModels[currentIndex].progressionState)
        {
            case ProgressionState.None:
                break;
            case ProgressionState.Forward:

                currentIndex = (currentIndex + 1) % DirectorModels.Count;

                Debug.Log("Progressing to Sequence " + currentIndex);
                
                Play(currentIndex);

                break;
            case ProgressionState.Direct:

                int targetIndex = DirectorModels[currentIndex].targetIndex;

                Play(targetIndex);

                break;
            default:
                break;
        }

    }

    public void Play(int targetIndex)
    {
        if (Director.Equals(null) || status.Equals(Status.Unitialized))
            return;

        DirectorModels[targetIndex].directorEvent.OnInit?.Invoke();

        Director.playableAsset = DirectorModels[targetIndex].timeline;

        switch (DirectorModels[targetIndex].currentWrapMode)
        {
            case DirectorWrapMode.Hold:

                SetDirectorToHold();

                break;
            case DirectorWrapMode.Loop:

                SetDirectorToLoop();

                break;
            case DirectorWrapMode.None:

                SetDirectorToNone();

                break;
            default:
                break;
        }

        currentIndex = targetIndex;

        Director.time = 0.0f;
        Director.Evaluate();

        Director.Play();

        DirectorModels[currentIndex].directorEvent.OnPlay?.Invoke();

        playStatus = PlayStatus.Playing;

    }

    public void Pause()
    {
        if (Director.Equals(null) || Director.playableAsset.Equals(null))
            return;

        Director.Pause();

        DirectorModels[currentIndex].directorEvent.OnPause?.Invoke();

        playStatus = PlayStatus.Paused;
    }

    public void Resume()
    {
        if (Director.Equals(null) || Director.playableAsset.Equals(null))
            return;

        Director.Resume();

        playStatus = PlayStatus.Playing;
    }



    private void SetDirectorToNone() => SetDirectorWrapState(DirectorWrapMode.None);


    private void SetDirectorToLoop() => SetDirectorWrapState(DirectorWrapMode.Loop);

    private void SetDirectorToHold() => SetDirectorWrapState(DirectorWrapMode.Hold);


    private void SetDirectorWrapState(DirectorWrapMode directorWrapMode)
    {
        if (Director.Equals(null))
            return;

        Director.extrapolationMode = directorWrapMode;
    }

    public void SetNone() => SetDirectorLoopState(DirectorWrapMode.None);

    public void SetHold() => SetDirectorLoopState(DirectorWrapMode.Hold);

    public void SetLoop() => SetDirectorLoopState(DirectorWrapMode.Loop);

    public void SetDirectorLoopState(DirectorWrapMode wrapMode)
    {
        Director.extrapolationMode = wrapMode;
    }
}
