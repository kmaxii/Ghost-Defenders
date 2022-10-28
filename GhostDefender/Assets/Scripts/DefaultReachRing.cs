using System.Collections;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

public class DefaultReachRing : ReachRing
{
    [SerializeField] private Transform parent;
 

    public void SetDefault(FloatVariable floatVariable)
    {
        float radiusItShouldBe = floatVariable.value / (parent.transform.localScale.x / 2);
        float diameterItShouldBe = radiusItShouldBe * 2;
        Vector3 scale = new Vector3(diameterItShouldBe, diameterItShouldBe, diameterItShouldBe);
        transform.localScale = scale;
    }
    
}
