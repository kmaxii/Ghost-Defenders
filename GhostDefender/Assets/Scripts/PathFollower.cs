using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PathFollower : MonoBehaviour
{
    protected List<Vector3> _path;

    public List<Vector3> Path
    {
        set => _path = value;
    }

    [HideInInspector] public int lives = 1;
    [SerializeField] protected IntVariable playerLives;
    [SerializeField] protected IntVariable money;
    [SerializeField] public SetReference balloonsSet;


    [HideInInspector] public float distanceTraveled;
    [HideInInspector] public int currentPos = 1;

    protected Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;

    public Spawnable spawnable;

    private readonly float _speedScale = 10f;

    protected bool _isCamo;
    protected bool _isRegen;

    public Vector2Int movingDirection;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        
        if (_path == null)
        {
            return;
        }

        var position = transform.position;

        bool changedDirection = false;

        if (position == _path[currentPos])
        {
            currentPos++;
            if (currentPos == _path.Count)
            {
                //Reached end
                playerLives.Value -= TotalLives;

                DestroyItself();
                return;
            }

            changedDirection = true;
        }

        Vector3 newPos = Vector3.MoveTowards(position, _path[currentPos], spawnable.speed / _speedScale);

        
        if (changedDirection)
        {
            movingDirection = Vector2Int.RoundToInt((newPos - position).normalized);
        }
        
        MoveToPosition(newPos);

        distanceTraveled =
            ((_path[currentPos] - position).magnitude / (_path[currentPos] - _path[currentPos - 1]).magnitude) +
            currentPos;

       
    }

    private int TotalLives
    {
        get
        {
            int total = lives;
            total += GetChildLives(spawnable.spawnOnDeath);
            return total;
        }
    }

    private int GetChildLives(WaveElement waveElement)
    {
        int livesInChild = 0;
        if (waveElement is BalloonCombination balloonCombination)
        {
            foreach (var waveElementWithDelay in balloonCombination.combination)
            {
                for (int i = 0; i < waveElementWithDelay.amount; i++)
                {
                    livesInChild += GetChildLives(waveElementWithDelay.waveElement);
                }
            }
        } else if (waveElement is Spawnable spawnable1)
        {
            livesInChild += spawnable1.lives;
            if (spawnable1.spawnOnDeath != null)
                livesInChild += GetChildLives(spawnable1.spawnOnDeath);
        }

        return livesInChild;

    }

    protected virtual void MoveToPosition(Vector2 newPos)
    {
        _rigidbody2D.MovePosition(newPos);
    }


    public virtual void Damage(int amount, BalloonAttribute[] attributes)
    {
        if (attributes != null)
        {
            foreach (var attributeWithInt in spawnable.damageModifier)
            {
                if (attributes.Contains(attributeWithInt.attribute))
                {
                    amount += attributeWithInt.damageAddition;
                }
            }
        }
        
        lives -= amount;
        if (lives <= 0)
        {
            Died();
        }
    }

    protected abstract void Died();


    protected void SpawnBalloonsAroundPosition(BalloonCombination balloonCombination)
    {
        Queue<Balloon> balloonsToSpawn =
            ConvertWaveElementQueueToBalloons(new Queue<WaveElementWithDelay>(balloonCombination.combination));

        //Split the damage between all spawned balloons
        int damageToDeal = lives / balloonsToSpawn.Count;

        float distance = 1f;

        float posOffset = 0 - distance * (balloonsToSpawn.Count / 2.0f);

        while (balloonsToSpawn.TryDequeue(out var balloon))
        {
            Vector3 posToSpawn = GetPositionAlongPath(posOffset, out int newPosToTrackTo);
            posOffset += distance;
            BalloonMono spawned = Spawner.BalloonPool.Get();
            spawned.transform.position = posToSpawn;
            //    Instantiate(gameObject, posToSpawn, Quaternion.identity).GetComponent<BalloonMono>();
            spawned.Path = _path;
            spawned.currentPos = newPosToTrackTo;
            spawned.SetUp(balloon, _isCamo, _isRegen);
            spawned.Damage(damageToDeal, null);
        }

        DestroyItself();
    }

    private Vector3 GetPositionAlongPath(float distance, out int currentPosCopy)
    {
        currentPosCopy = currentPos;

        var position = transform.position;

        if (distance == 0)
        {
            return position;
        }

        Vector3 posCopy = new Vector3(position.x, position.y, position.z);

        //While still in a turn
        while (true)
        {
            Vector3 direction = _path[currentPosCopy] - posCopy;
            float directionMagnitude = direction.magnitude;

            //Not in a turn
            if (distance <= directionMagnitude)
            {
                //Normalized the direction vector and adds distance to it to get the right point
                return position + ((direction / directionMagnitude) * distance);
            }

            //This means that the pos to spawn in is inside a turn
            posCopy = _path[currentPosCopy];
            currentPosCopy++;
            distance -= directionMagnitude;
        }
    }

    private static Queue<Balloon> ConvertWaveElementQueueToBalloons(Queue<WaveElementWithDelay> input)
    {
        Queue<Balloon> balloonQueue = new Queue<Balloon>();

        while (input.TryDequeue(out var result))
        {
            if (result.waveElement is BalloonCombination combination)
            {
                for (int i = 0; i < combination.combination.Count; i++)
                {
                    WaveElementWithDelay element = combination.combination[i];
                    for (int j = 0; j < element.amount; j++)
                    {
                        if (element.waveElement is Balloon balloon)
                        {
                            balloonQueue.Enqueue(balloon);
                        }
                        else
                        {
                            input.Enqueue(element);
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < result.amount; j++)
                {
                    balloonQueue.Enqueue((Balloon) result.waveElement);
                }
            }
        }

        return balloonQueue;
    }

    protected virtual void DestroyItself()
    {
        balloonsSet.Remove(gameObject);
        Destroy(gameObject);
    }
}