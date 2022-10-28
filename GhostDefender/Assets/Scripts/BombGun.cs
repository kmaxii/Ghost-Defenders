

using UnityEngine;

public class BombGun : Gun
{

    [Header("BombGun")]
    public float bombRadius;

    public Bomb.BombState bombState = Bomb.BombState.Normal;


    public void SetFragBomb() => bombState = Bomb.BombState.Frag;
    public void SetClusterBomb() => bombState = Bomb.BombState.Cluster;
    public void SetImpactBomb() => bombState = Bomb.BombState.Impact;
    
    
    protected override void SettingUpBullet(Bullet bullet)
    {
        base.SettingUpBullet(bullet);
        Bomb bomb = (Bomb)bullet;
        bomb.Radius = bombRadius;
        bomb.bombState = bombState;
    }
}
