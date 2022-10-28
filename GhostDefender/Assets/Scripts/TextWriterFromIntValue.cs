using System;
using Interfaces;
using Scriptable_objects;
using TMPro;
using UnityEngine;

public class TextWriterFromIntValue : MonoBehaviour, IEventListenerInterface
{
    [SerializeField] private IntVariable value;

    private TextMeshProUGUI _textMesh;


    
    private void OnEnable()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();

        OnEventRaised();
        value.raiseOnValueChanged.RegisterListener(this);
    }


    private void OnDisable()
    {
        value.raiseOnValueChanged.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _textMesh.text = value.Value.ToString();
    }
}
