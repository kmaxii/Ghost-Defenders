using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Bomb : Bullet
{
    public enum BombState
    {
        Normal,
        Frag,
        Cluster,
        Impact
    }

    [Header("Explosion")] 
    [Tooltip("Size of the circle where damage is dealt")]
    [SerializeField] private float radius = 3;

    [SerializeField] private GameObject frag;
    [SerializeField] private float fragSpeed;
    [FormerlySerializedAs("fragRange")] [SerializeField] private float fragMaxRange;
    
    [SerializeField] private GameObject clusterBomb;
    [SerializeField] private float clusterExplosionDistance = 2f;
    [SerializeField] private float clusterBombSpeed = 1000f;

    public BombState bombState = BombState.Normal;

    public float Radius
    {
        set => radius = value;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PathFollower>(out PathFollower pathFollower))
        {
            _pierce--;
            
     
            
            if (_pierce <= 0)
            {
                switch (bombState)
                {
                    case BombState.Normal:
                        Explosion();
                        break;
                    case BombState.Frag:
                        Explosion();
                        Shoot8Around(frag, fragSpeed, fragMaxRange, 1);
                        break;
                    case BombState.Cluster:
                        Explosion();
                        Shoot8Around(clusterBomb, clusterBombSpeed, clusterExplosionDistance, Int32.MaxValue);
                        break;
                    case BombState.Impact:
                        Explosion();
                        break;
                    
                }
                Destroy(gameObject);
            }
        }
    }


    protected void Explosion()
    {
        TriggerSoundAndAnimation(transform.position);

        var hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            BalloonMono balloonMono = hit.GetComponent<BalloonMono>();
            DamageBalloon(balloonMono);
        }
    }

    
    private void Shoot8Around(GameObject obj, float speed, float range, int pierce)
    {
        Vector2 dir = Vector2.down;
        
        int randomRotation = Random.Range(0, 360);
        dir = RotateRadians(dir, randomRotation * Mathf.PI / 180f);
        
        Quaternion rotation = Quaternion.Euler(0,0, randomRotation);;
        
        
        for (int i = 0; i < 8; i++)
        {
            Shoot(dir, rotation, obj, speed, range, pierce);
            
            float rotationInRad = Mathf.PI / 4.0f;
            
            dir = RotateRadians(dir, rotationInRad);

            rotation *= Quaternion.Euler(0,0, 45);
        }
    }
    
    private static Vector2 RotateRadians(Vector2 v, float radians)
    {
        float ca = Mathf.Cos(radians);
        float sa = Mathf.Sin(radians);
        return new Vector2(ca * v.x - sa * v.y, sa*v.x + ca*v.y);
    }
    
    private void Shoot(Vector3 direction, Quaternion rotation, GameObject obj, float speed, float range, int pierce)
    {
        direction *= speed * 1.0f;
        
        GameObject spawned = Instantiate(obj, transform.position, rotation);

        Bullet bullet = spawned.GetComponent<Bullet>();
        
        bullet.Setup(1, pierce, range, _balloonsPopped);

        spawned.GetComponent<Rigidbody2D>().AddForce(direction);
    }


  
}
