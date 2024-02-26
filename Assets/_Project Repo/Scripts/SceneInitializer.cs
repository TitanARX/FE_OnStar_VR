using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneInitializer : MonoBehaviour
{
    public UnityEvent OnInitializationScene;
    public UnityEvent OnSceneReady;


    private IEnumerator Start()
    {
        OnSceneReady?.Invoke();

        yield return new WaitForSeconds(1.5f);

        OnInitializationScene.Invoke();
    }



}
