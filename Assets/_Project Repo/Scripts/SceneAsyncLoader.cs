using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAsyncLoader : MonoBehaviour
{
    
    [SerializeField]
    private string _levelToLoad = "Scenario 1";


    public void LoadScene()
    {
        Debug.LogErrorFormat("Loading Scene");

        SceneManager.LoadScene(_levelToLoad);
    }
   
}
