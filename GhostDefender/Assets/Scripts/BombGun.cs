

using UnityEngine;

public class BombGun : Gun
{

    [Header("BombGun")]
    public float bombRadius;
    
    
    protected override void SettingUpBullet(Bullet bullet)
    {
        base.SettingUpBullet(bullet);
        Bomb bomb = (Bomb)bullet;
        bomb.Radius = bombRadius;
        bomb._maxRange = (transform.position - _shootingTower.furthestInRange.transform.position).magnitude - 1;
    }
}
