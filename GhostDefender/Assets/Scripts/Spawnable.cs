using System;
using UnityEngine;

public abstract class Spawnable : WaveElement
{
    [Header("Spawnable")]
    [Tooltip("Hits required to die")]
    public int lives;
    [Tooltip("The speed it travels. multiplied with the default base speed. With this, 1 is default")]
    public float speed = 1f;

    [Tooltip("A balloon or balloon combination to spawn on death")]
    public WaveElement spawnOnDeath;
    
    [Tooltip("Money gotten on pop. This will only be gotten the first time this is popped in case of regen")]
    public int moneyOnPop;

    [Header("Attributes")] 
    [Header("The bullet must have these attributes to damage it")]
    public BalloonAttribute[] mustHaveToPop;

    [Header("The bullet can't have any of these attributes to damage it")]
    public BalloonAttribute[] cantHaveToPop;
    
    [Header("Damage bonus (or withdraw) a bullet with some attribute gets")]
    public AttributeWithInt[] damageModifier;

    private void OnValidate()
    {
        if (speed <= 0f)
        {
            speed = 0.01f;
            Debug.LogError("Speed can't be negative. ", this);
        }
    }
}
[Serializable]
public class AttributeWithInt
{
    public BalloonAttribute attribute;
    public int damageAddition;
}