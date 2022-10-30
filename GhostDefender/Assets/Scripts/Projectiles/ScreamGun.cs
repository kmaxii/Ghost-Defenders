using System.Collections;
using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;
using UnityEngine.Assertions;

public class ScreamGun : Gun
{
    private float bulletSize;
    [SerializeField] private AudioEvent audioEvent;
    [SerializeField] private GameEventWithAudioEvent gEvent;
    
    protected override void SettingUpBullet(Bullet bullet)
    {
        gEvent.Raise(audioEvent);
        Assert.IsTrue(bullet is SoundWave);
        SoundWave soundWave = (SoundWave) bullet;

        soundWave.maxScale += bulletSize;
    }

    public void SetBulletMaxSize(float newSize)
    {
        bulletSize = newSize;
    }
}
