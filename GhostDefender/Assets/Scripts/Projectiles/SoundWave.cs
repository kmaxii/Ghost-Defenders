using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundWave : SharpBullet
{
    private int _maxDamage;

    private float _baseScale;
    
    public float maxScale = 1;

    private void Start()
    {
        _maxDamage = _damage;
        _baseScale = transform.localScale.x;
        if (maxScale < _baseScale)
        {
            maxScale = _baseScale;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float procDistanceTraveled = (_startingPos - transform.position).magnitude / _maxRange;
        if (procDistanceTraveled > 1f)
        {
            procDistanceTraveled = 1f;
        }
        _damage = 1 + (int) ( (_maxDamage - 1) * (1 - procDistanceTraveled) );
        float newScale = _baseScale + (maxScale-_baseScale) * procDistanceTraveled;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
