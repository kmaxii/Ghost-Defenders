using Interfaces;
using Scriptable_objects;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour, IEventListenerInterface
{
    [SerializeField] private Animator animator;

    private static readonly int ShowScreen1 = Animator.StringToHash("ShowScreen");

    [SerializeField] private IntVariable lives;
    [SerializeField] private GameObject enableOnShow;
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        lives.raiseOnValueChanged.RegisterListener(this);
    }

    private void OnDestroy()
    {
        lives.raiseOnValueChanged.UnregisterListener(this);
    }


    private void ShowScreen()
    {
        enableOnShow.SetActive(true);
        animator.SetTrigger(ShowScreen1);
    }

    public void OnEventRaised()
    {

        if (lives.Value == 0)
        {
            text.text = "GAME OVER";
            ShowScreen();

        }
    }

    public void WonGame()
    {
        text.text = "YOU WON";
        ShowScreen();
    }
}
