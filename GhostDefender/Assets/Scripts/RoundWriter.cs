using Interfaces;
using Scriptable_objects;
using TMPro;
using UnityEngine;

public class RoundWriter : MonoBehaviour, IEventListenerInterface
{

    private int round = 0;

    [SerializeField] private GameEvent gameEvent;

    private TextMeshProUGUI _text;
    
    // Start is called before the first frame update
    void Start()
    {
        gameEvent.RegisterListener(this);
        _text = GetComponent<TextMeshProUGUI>();
        OnEventRaised();
    }
    

    public void OnEventRaised()
    {
        round++;
        _text.text = $"Round {round}/40";
    }
}
