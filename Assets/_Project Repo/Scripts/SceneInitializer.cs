using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneInitializer : MonoBehaviour
{
    public UnityEvent OnInitializationScene;
    public UnityEvent OnSceneReady;

    public float startDelay = 1.5f;


    private IEnumerator Start()
    {
        OnSceneReady?.Invoke();

        yield return new WaitForSeconds(startDelay);

        OnInitializationScene.Invoke();
    }



}
