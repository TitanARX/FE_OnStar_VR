using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMatch : MonoBehaviour
{
    //Hack will replace with better UI Elements
    public TextMeshProUGUI _tmpro;
    public UnityEngine.UI.Text _text;

   

    private void Update()
    {
        _tmpro.text = _text.text;
    }
}
