using System;
using Scriptable_objects;
using UnityEngine;

public class StatsResetter : MonoBehaviour
{
    [NonReorderable]
    [SerializeField] private IntVariableAndInt[] toSetArray;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var toSet in toSetArray)
        {
            toSet.intVariable.Value = toSet.value;
        }
    }
}

[Serializable]
public struct IntVariableAndInt
{
    public int value;
    public IntVariable intVariable;
}