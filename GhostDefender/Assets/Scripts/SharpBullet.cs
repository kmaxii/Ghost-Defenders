using UnityEngine;

public class SharpBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PathFollower>(out PathFollower pathFollower))
        {
            if (!CanDamage(pathFollower))
                return;
            
            DamageBalloon(pathFollower);

            TriggerSoundAndAnimation(other.transform.position);

            _pierce--;
            if (_pierce <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}