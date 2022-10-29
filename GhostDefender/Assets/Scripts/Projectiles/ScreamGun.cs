using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ScreamGun : Gun
{
    private float bulletSize;

    protected override void SettingUpBullet(Bullet bullet)
    {
        Assert.IsTrue(bullet is SoundWave);
        SoundWave soundWave = (SoundWave) bullet;

        soundWave.maxScale += bulletSize;
    }

    public void SetBulletMaxSize(float newSize)
    {
        bulletSize = newSize;
    }
}
