using System;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

public class BalloonMono : PathFollower
{
    private Balloon _balloon;
    
    private HashSet<Balloon> _hasGottenMoneyFrom;
    private Stack<Balloon> _previousBalloons;


    [SerializeField] private float timeToRegenDefault;
    private float _timeToRegen;
    

    public void ResetHasGottenMoneyFrom()
    {
        if (_hasGottenMoneyFrom == null)
        {
            _hasGottenMoneyFrom = new HashSet<Balloon>();
            _previousBalloons = new Stack<Balloon>();
            
        }
        else
        {
            _hasGottenMoneyFrom.Clear();
            _previousBalloons.Clear();
        }

        _balloon = null;
    }
    

    public void SetUp(Balloon balloon, bool isCamo, bool isRegen)
    {
        lives = balloon.lives;
        if (isCamo)
        {
            _spriteRenderer.sprite = isRegen ? balloon.regenCamoTexture : balloon.camoTexture;
        }
        else if (isRegen)
        {
            _spriteRenderer.sprite = balloon.regenTexture;
        }
        else
        {
            _spriteRenderer.sprite = balloon.balloonTexture;
        }
        
        _balloon = balloon;
        _isCamo = isCamo;
        _isRegen = isRegen;
        _timeToRegen = timeToRegenDefault;
        spawnable = _balloon;
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        if (_path == null || _balloon == null)
        {
            return;
        }
        
        base.Update();
        
        if (_isRegen)
        {
            _timeToRegen -= Time.deltaTime;
            if (_timeToRegen < 0 && _previousBalloons.Count >= 1)
            {
                SetUp(_previousBalloons.Pop(), _isCamo, _isRegen);
            }
        }

        
    }


    protected override void Died()
    {

        //Can only get money once, so regen balloons aren't infinite money
        if (_isRegen)
        {
            if (!_hasGottenMoneyFrom.Contains(_balloon))
            {
                money.Value += _balloon.moneyOnPop;
                _hasGottenMoneyFrom.Add(_balloon);
            }
        }
        else
        {
            money.Value += _balloon.moneyOnPop;
        }
        if (_balloon.spawnOnDeath)
        {
            _previousBalloons.Push(_balloon);

            if (_balloon.spawnOnDeath is Balloon newBalloon)
            {
                if (lives < 0)
                {
                    while (newBalloon.lives + lives <= 0)
                    {
                        lives += newBalloon.lives;
                        money.Value += newBalloon.moneyOnPop;
                        if (newBalloon.spawnOnDeath)
                        {
                            if (newBalloon.spawnOnDeath is Balloon newestBalloon)
                            {
                                newBalloon = newestBalloon;
                                continue;
                            }

                            SpawnBalloonsAroundPosition((BalloonCombination) newBalloon.spawnOnDeath);
                            DestroyItself();
                            return;
                        }
                        DestroyItself();
                    }
                }

                SetUp(newBalloon, _isCamo, _isRegen);
                return;
            }

            SpawnBalloonsAroundPosition((BalloonCombination) _balloon.spawnOnDeath);
            return;
        }

        DestroyItself();
    }

    protected override void DestroyItself()
    {
        Spawner.BalloonPool.Release(this);
    }
}