using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Bomb : Bullet
{

    [Header("Explosion")] 
    [Tooltip("Size of the circle where damage is dealt")]
    [SerializeField] private float radius = 3;

    public float Radius
    {
        set => radius = value;
    }

    private void OnDestroy()
    {
        Explosion();
    }


    protected void Explosion()
    {
        TriggerSoundAndAnimation(transform.position);

        var hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            PrefabEnemyMono balloonMono = hit.GetComponent<PrefabEnemyMono>();
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
