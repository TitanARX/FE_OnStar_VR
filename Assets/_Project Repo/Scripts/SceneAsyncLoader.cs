using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAsyncLoader : MonoBehaviour
{
    [SerializeField]
    private OVRScreenFade screenFade;

    [SerializeField]
    private string _levelToLoad = "Scenario 1";

    private void OnValidate()
    {
        screenFade = FindObjectOfType<OVRScreenFade>();
    }

    private void Awake()
    {
        if (screenFade == null)
            return;

        screenFade.fadeOnStart = true;
    }

    public void LoadScene()
    {
        if (screenFade || _levelToLoad == string.Empty)
            return;

        Sequence _loadSequence = DOTween.Sequence();

        _loadSequence.PrependCallback(() => screenFade.FadeOut())
                        .AppendInterval(1.75f)
                            .OnComplete(() => SceneManager.LoadScene(_levelToLoad));
    }
}
