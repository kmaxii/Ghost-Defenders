using System;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

public class BlimpMono : PathFollower
{
    [SerializeField] 
    [NonReorderable]
    [Tooltip("change texture if lives remaining are equal to this")]
    private IntWithSprite[] textures;

    private Dictionary<int, Sprite> _changeTexture;


    private void Start()
    {
        _changeTexture = new Dictionary<int, Sprite>();
        foreach (var intWithSprite in textures)
        {
            _changeTexture.Add(intWithSprite.value, intWithSprite.sprite);
        }
        
        balloonsSet.Add(gameObject);
    }

    protected override void MoveToPosition(Vector2 newPos)
    {
        base.MoveToPosition(newPos);
        Vector2 direction = ((Vector2) transform.position - newPos).normalized;
        
        float angle = direction == Vector2.up ? -90 : Vector2.Angle(Vector2.left, direction);
        
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
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

    public override void Damage(int amount, BalloonAttribute[] attribute)
    {
        base.Damage(amount, null);
        if (lives > 0 && _changeTexture.ContainsKey(lives))
        {
            _spriteRenderer.sprite = _changeTexture[lives];
        }
    }
}

[Serializable]
public struct IntWithSprite
{
    public int value;
    public Sprite sprite;
}