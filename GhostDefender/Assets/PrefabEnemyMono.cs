using System;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

public class PrefabEnemyMono : PathFollower
{
    private Animator _animator;

    private void Start()
    {
        balloonsSet.Add(gameObject);
        _animator = GetComponent<Animator>();
    }
    
    
    protected override void ChangedDirection()
    {
        base.ChangedDirection();
        switch (movingDirection)
        {
            case var v when v.Equals(Vector2Int.right):
                _animator.SetTrigger("WalkRight");
                _spriteRenderer.flipX = false;
                break;
            case var v when v.Equals(Vector2Int.left):
                _animator.SetTrigger("WalkRight");
                _spriteRenderer.flipX = true;
                break;
            case var v when v.Equals(Vector2Int.up):
                _animator.SetTrigger("WalkAway");
                break;
            case var v when v.Equals(Vector2Int.down):
                _animator.SetTrigger("WalkTo");
                break;
        }
    }

 /*   protected override void MoveToPosition(Vector2 newPos)
    {
        base.MoveToPosition(newPos);
        Vector2 direction = ((Vector2) transform.position - newPos).normalized;
        
        float angle = direction == Vector2.up ? -90 : Vector2.Angle(Vector2.left, direction);
        
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
    */
    public void SetUp(Blimp blimp, bool isCamo, bool isRegen)
    {
        lives = blimp.lives;

        _isCamo = isCamo;
        _isRegen = isRegen;
        spawnable = blimp;
    }


    protected override void Died()
    {
        money.Value += spawnable.moneyOnPop;
        balloonsSet.Remove(gameObject);

        if (!spawnable.spawnOnDeath)
        {
            DestroyItself();
            return;
        }

        if (spawnable.spawnOnDeath is BalloonCombination combo)
        {
            SpawnBalloonsAroundPosition(combo);
            return;
        }


        if (spawnable.spawnOnDeath is Balloon newBalloon)
        {
            if (lives >= 0)
            {
                Spawner.Instance.InstantiateBalloon(newBalloon, _isCamo, _isRegen);
                return;
            }
            
            while (newBalloon.lives + lives <= 0)
            {
                lives += newBalloon.lives;
                money.Value += newBalloon.moneyOnPop;
                if (newBalloon.spawnOnDeath)
                {
                    if (newBalloon.spawnOnDeath is Balloon newestBalloon)
                    {
                        Spawner.Instance.InstantiateBalloon(newestBalloon, _isCamo, _isRegen);
                        continue;
                    }

                    SpawnBalloonsAroundPosition((BalloonCombination) newBalloon.spawnOnDeath);
                    DestroyItself();
                    return;
                }

                DestroyItself();
            }
        }
    }
    
}

[Serializable]
public struct IntWithSprite
{
    public int value;
    public Sprite sprite;
}