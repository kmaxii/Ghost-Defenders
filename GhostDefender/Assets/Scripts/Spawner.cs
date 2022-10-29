using System.Collections.Generic;
using Interfaces;
using Scriptable_objects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour, IChildFinished, IEventListenerInterface
{
    [Header("Path data")] [SerializeField] private Tilemap tilemap;
    [SerializeField] private Direction startingDirection = Direction.Right;

    private List<Vector3> _path;

    [Header("Wave data")] [SerializeField] private BalloonCombination balloonCombination;

    private int _currentLevel;

    [Tooltip("Game events to trigger when wave is finished.")] [SerializeField]
    private GameEvent triggerOnRoundFinished;

    [Tooltip("The event that is called when the balloons set is empty")] [SerializeField]
    private GameEvent balloonsSetEmptied;
    
    
    [Tooltip("The event that is called when there is no more rounds")] [SerializeField]
    private GameEvent gameWon;

    [SerializeField] private GameObject balloonPrefab;

    private float _currentDelay;
    private bool _everythingSpawned;

    public static ObjectPool<BalloonMono> BalloonPool;

    [SerializeField] private RoundStart toggleOnWhenRoundFinished;

    [Header("Balloon Pool")] [SerializeField]
    private int defaultCapacity = 200;

    [SerializeField] private int maxSize = 500;

    public static Spawner Instance;

    [SerializeField] private SetReference _ballonSet;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        Vector3Int startingCell = tilemap.layoutGrid.WorldToCell(transform.position);
        if (!tilemap.HasTile(startingCell))
        {
            Debug.LogWarning("There needs to be a tile at spawn location", this);
            return;
        }

        InitializePath(startingCell);
        SetUpPool();

        balloonsSetEmptied.RegisterListener(this);
    }


    private void SetUpPool()
    {
        BalloonPool = new ObjectPool<BalloonMono>(
            () => Instantiate(balloonPrefab).GetComponent<BalloonMono>()
            , follower =>
            {
                GameObject o;
                (o = follower.gameObject).SetActive(true);
                follower.balloonsSet.Add(o);
                follower.currentPos = 0;
                follower.distanceTraveled = 0;
                follower.ResetHasGottenMoneyFrom();
                follower.Path = _path;
            }, follower =>
            {
                GameObject o;
                (o = follower.gameObject).SetActive(false);
                follower.balloonsSet.Remove(o);
            }, follower => Destroy(follower.gameObject)
            , false
            , defaultCapacity,
            maxSize);
    }

    public void SendNextWave()
    {
        _everythingSpawned = false;
        SpawnBalloonCombination spawnBalloonCombination =
            new SpawnBalloonCombination(balloonCombination.combination[_currentLevel], this, this);
        spawnBalloonCombination.Start();
        _currentLevel++;
    }


    public void InstantiateBalloon(Spawnable spawnable, bool isCamo, bool isRegen)
    {
        if (spawnable is Balloon balloon)
        {
            var pathFollower = BalloonPool.Get();
            pathFollower.transform.position = transform.position;
            pathFollower.Path = _path;
            pathFollower.SetUp(balloon, isCamo, isRegen);
            return;
        }

        if (spawnable is Blimp blimp)
        {
            PrefabEnemyMono prefabEnemyMono = Instantiate(blimp.blimp).GetComponent<PrefabEnemyMono>();
            prefabEnemyMono.Path = _path;
            prefabEnemyMono.transform.position = transform.position;
            prefabEnemyMono.SetUp(blimp, isCamo, isRegen);
            return;
        }

        Debug.LogError("Unknown type sent in. " + spawnable.GetType().ToString(), this);
    }


    private void InitializePath(Vector3Int position)
    {
        _path = new List<Vector3>();
        _path.Add(transform.position);
        Vector3Int direction = (Vector3Int) DirectionToVector(startingDirection);
        while (true)
        {
            //Continue going straight if there is a path tile there
            if (tilemap.HasTile(position + direction))
            {
                position += direction;
                continue;
            }

            bool foundPath = false;

            //Check both other directions
            for (int i = 1; i <= 2; i++)
            {
                Vector3Int newDirection = GetOtherDirectionToLook(direction, i);
                Vector3Int checkingPos = position + newDirection;

                if (tilemap.HasTile(checkingPos))
                {
                    direction = newDirection;
                    AddToPath(tilemap.layoutGrid.CellToWorld(position));
                    position = checkingPos;
                    foundPath = true;

                    break;
                }
            }

            if (!foundPath)
            {
                AddToPath(tilemap.layoutGrid.CellToWorld(position));
                break;
            }
        }
    }

    private void AddToPath(Vector3 point)
    {
        point.x += 0.5f;
        point.y += 0.5f;
        _path.Add(point);
    }

    private Vector2Int DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                return Vector2Int.right;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Up:
                return Vector2Int.up;
        }

        return Vector2Int.zero;
    }

    private Vector3Int GetOtherDirectionToLook(Vector3Int direction, int getFirst)
    {
        if (direction.x != 0)
        {
            if (getFirst == 1)
            {
                return new Vector3Int(0, 1, 0);
            }

            return new Vector3Int(0, -1, 0);
        }

        if (getFirst == 1)
        {
            return new Vector3Int(1, 0, 0);
        }

        return new Vector3Int(-1, -0, 0);
    }


    enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    //This is called when there are no more balloons to spawn
    public void Continue(SpawnBalloonCombination triggeredBy)
    {
        _everythingSpawned = true;
    }

    //This is called when there are no more balloons on the screen.
    public void OnEventRaised()
    {
        if (_everythingSpawned)
        {
            Invoke(nameof(ShowRoundFinishedButton), 0.5f);
        }
    }

    private void ShowRoundFinishedButton()
    {
        if (_ballonSet.IsEmpty())
        {
            toggleOnWhenRoundFinished.ShowButton();
            triggerOnRoundFinished.Raise();
        }
    }
}