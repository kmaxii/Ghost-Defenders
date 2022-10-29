using System.Collections.Generic;
using System.Linq;
using Scriptable_objects;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected int _damage = 1;

    [Tooltip("Pierce is the amount of balloons it can damage before dying itself")] [SerializeField]
    protected float rangeOverwrite;

    protected int _pierce = 1;

    protected float _maxRange;

    protected Vector3 _startingPos;
    protected IntVariable _balloonsPopped;

    [SerializeField] private AudioEvent audioEvent;
    [SerializeField] private GameEventWithAudioEvent @event;
    [SerializeField] private SpriteAnimation animationOnHit;

    [SerializeField] private BalloonAttribute[] attributes;


    public void Setup(int damage, int pierce, float maxRange, IntVariable balloonsPopped)
    {
        this._damage = damage;
        this._pierce = pierce;

        _startingPos = transform.position;

        if (rangeOverwrite != 0)
        {
            _maxRange = rangeOverwrite;
        }
        else
        {
            this._maxRange = maxRange;
        }

        _balloonsPopped = balloonsPopped;
    }

    
    protected void DamageBalloon(PathFollower pathFollower)
    {
        if (!CanDamage(pathFollower))
            return;
        pathFollower.Damage(_damage, attributes);
        _balloonsPopped.Value++;
    }

    protected bool CanDamage(PathFollower pathFollower)
    {
        Spawnable spawnable = pathFollower.spawnable;

        int mustHaveContains = 0;
        foreach (var t in attributes)
        {
            if (spawnable.cantHaveToPop.Contains(t))
            {
                return false;
            }
            
            if (spawnable.mustHaveToPop.Contains(t))
            {
                mustHaveContains++;
            }
        }
        return mustHaveContains == spawnable.mustHaveToPop.Length;
    }
    protected virtual void FixedUpdate()
    {
        if ((_startingPos - transform.position).sqrMagnitude > _maxRange * _maxRange)
        {
            Destroy(gameObject);
        }
    }

    protected void TriggerSoundAndAnimation(Vector3 position)
    {
        @event.Raise(audioEvent);
        AnimationPool.Instance.Spawn(animationOnHit, position);
    }
}