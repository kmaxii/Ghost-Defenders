using System;
using Pathfinding;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PrefabEnemyMono : PathFollower
{
    private Animator _animator;

    private bool _isPathfidinigOut;
    
    [SerializeField] private AudioEvent deathAudio;
    [SerializeField] private GameEventWithAudioEvent @event;

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
                _spriteRenderer.flipX = true;
                break;
            case var v when v.Equals(Vector2Int.left):
                _animator.SetTrigger("WalkRight");
                _spriteRenderer.flipX = false;
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

    public override void Damage(int amount, BalloonAttribute[] attributes)
    {
        base.Damage(amount, attributes);
        _spriteRenderer.color = Color.gray;
        Invoke(nameof(ResetColor), 0.2f);
    }

    private void ResetColor()
    {
        if (_isPathfidinigOut)
            return;
        _spriteRenderer.color = Color.white;
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

    protected override void DestroyItself()
    {
        if (_isPathfidinigOut)
        {
            Destroy(gameObject);
            return;
        }
        balloonsSet.Remove(gameObject);
        PathFindOut();
    }

    protected override void MakePlayerLoseHealth()
    {
        if (!_isPathfidinigOut)
            base.MakePlayerLoseHealth();
    }


    private void PathFindOut()
    {
        @event.Raise(deathAudio);
        _speedScale = 7f;
        gameObject.tag = "Untagged";
        GetComponent<CapsuleCollider2D>().enabled = false;
        Invoke(nameof(SetTransparent), 0.1f);

        Tilemap tilemap = GridManager.Instance.GetTileMap("Objects");

        Vector3Int startingPos = tilemap.WorldToCell(transform.position);
        Vector3Int endPos = tilemap.WorldToCell(Spawner.Instance.transform.position);

        AStar aStar = new AStar(startingPos, endPos, tilemap);
        aStar.RunAlgorithm();
        _path = aStar.GetPath();
        currentPos = 1;
        _isPathfidinigOut = true;
    }

    private void SetTransparent()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}

[Serializable]
public struct IntWithSprite
{
    public int value;
    public Sprite sprite;
}