using Interfaces;
using Scriptable_objects;
using TMPro;
using UnityEngine;

public class TextWriterFromIntValueWithStart : MonoBehaviour, IEventListenerInterface
{
    private IntVariable _value;

    private TextMeshProUGUI _textMesh;


    

    public void Setup(IntVariable intVariable)
    {
        if (!_textMesh)
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }
        _value = intVariable;
        OnEventRaised();
        _value.raiseOnValueChanged.RegisterListener(this);
    }


    private void OnDisable()
    {
        _value.raiseOnValueChanged.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        int value = _value.Value;
        _textMesh.text = value.ToString();
    }
}
