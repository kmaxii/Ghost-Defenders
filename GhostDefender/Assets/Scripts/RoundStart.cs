using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class RoundStart : MonoBehaviour
{
    private int _round = 1;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameOver gameOver;

    [NonReorderable] [SerializeField] private AudioClip[] changeSongsOnRoundsArray;

    public void Start()
    {
        ShowButton();
    }

    public void ShowButton()
    {
        if (_round == 13)
        {
            gameOver.WonGame();
            return;
        }
        transform.parent.gameObject.SetActive(true);
        text.text = $"Round {_round}";
        _round++;
    }


    public void HideButton()
    {
        transform.parent.gameObject.SetActive(false);

        var randomIndex = new Random().Next(0, changeSongsOnRoundsArray.Length);
        MusicManager.FadeToSong(changeSongsOnRoundsArray[randomIndex]);
        Debug.Log(randomIndex);

    }
}
