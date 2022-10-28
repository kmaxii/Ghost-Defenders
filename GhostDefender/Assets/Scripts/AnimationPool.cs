using MonoBehaviours;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Pool;

public class AnimationPool : MonoBehaviour
{
    private ObjectPool<SpriteAnimationMonoBehaviour> _pool;

    public static AnimationPool Instance;

    public GameObject animationPrefab;

    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxSize;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        SetUpPool();
    }
    
    private void SetUpPool()
    {
        
        _pool = new ObjectPool<SpriteAnimationMonoBehaviour>(
            () => Instantiate(animationPrefab).GetComponent<SpriteAnimationMonoBehaviour>()
            , spriteAnimation => spriteAnimation.gameObject.SetActive(true)
            , spriteAnimation =>
            {
                spriteAnimation.gameObject.SetActive(false);
            }, spriteAnimation => Destroy(spriteAnimation.gameObject)
            , false
            , defaultCapacity, 
            maxSize);
    }

    public void Spawn(SpriteAnimation spriteAnimation, Vector3 position)
    {
        SpriteAnimationMonoBehaviour spriteAnimationMono = _pool.Get();

        spriteAnimationMono.PlayAnimation(spriteAnimation, position); 
    }

    public void Return(SpriteAnimationMonoBehaviour toReturn)
    {
        _pool.Release(toReturn);
    }
    
}

























