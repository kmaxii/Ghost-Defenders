using System.Collections;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Serialization;

public class AddMoneyOnRoundFinished : MonoBehaviour
{

    [SerializeField] private IntVariable money;

    [SerializeField] private int baseAward = 100;
    [SerializeField] private int increaseEachRound = 1;

    private int _round;



    public void ApplyAward()
    {
        _round++;
        money.Value += baseAward + (increaseEachRound * _round);
    }
}
