using System;
using System.Collections;
using System.Numerics;
using Scriptable_objects;
using Towers;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Gun : MonoBehaviour
{
    [Tooltip("The first element of this array is the starting bullet. The active one can be decided with the activeBullet Int")]
    [SerializeField] protected GameObject[] bullets;
    public int activeBullet;
    [Tooltip("How many degrees offset the bullet should be shoot compared to targets position. (Clockwise).")]
    [Range(0, 360)]
    [SerializeField] private float offset;
    [Tooltip("How much extra delay should be added for the Gun to fire.")]
    [SerializeField] private float delay;

    private float _offsetInRadians;

    private ShootingTower _shootingTower;


    public void SetActiveBullet(int i)
    {
        activeBullet = i;
    }
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _offsetInRadians = offset * Mathf.Deg2Rad;
        _shootingTower = transform.parent.GetComponent<ShootingTower>();
        _shootingTower.ShootEvent += Shoot;
    }

    private void OnDestroy()
    {
        _shootingTower.ShootEvent -= Shoot;
    }
    

    private void Shoot(int damage, int bulletSpeed, int pierce, float range, IntVariable balloonsPopped)
    {
        StartCoroutine(ShootWithDelay(damage, bulletSpeed, pierce, range, _shootingTower.furthestInRange.transform.position, balloonsPopped));

    }

    private IEnumerator ShootWithDelay(int damage, int bulletSpeed, int pierce, float range, Vector3 furthestInRangeCopy, IntVariable balloonsPopped)
    {
        yield return new WaitForSeconds(delay);
        var position = transform.position;
        
        Vector3 direction;

        if (_shootingTower.furthestInRange)
        {
            direction = _shootingTower.furthestInRange.transform.position - position;
        }
        else
        {
            direction = furthestInRangeCopy - position;
        }

        direction.Normalize();

        if (offset != 0)
        {
            direction = RotateRadians(direction, _offsetInRadians);
        }

        direction *= (bulletSpeed * 1.0f);
        
        GameObject spawned = Instantiate(bullets[activeBullet], position, Quaternion.FromToRotation(Vector3.down, direction));

        Bullet bullet = spawned.GetComponent<Bullet>();
        
        bullet.Setup(damage, pierce, range, balloonsPopped);
        SettingUpBullet(bullet);

        spawned.GetComponent<Rigidbody2D>().AddForce(direction);
    }

    protected virtual void SettingUpBullet(Bullet bullet)
    {
        
    }

    private static Vector3 RotateRadians(Vector3 v, float radians)
    {
        float ca = Mathf.Cos(radians);
        float sa = Mathf.Sin(radians);
        return new Vector3(ca * v.x - sa * v.y, sa*v.x + ca*v.y, v.z);
    }
}
