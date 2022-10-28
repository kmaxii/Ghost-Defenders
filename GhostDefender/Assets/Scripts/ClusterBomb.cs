public class ClusterBomb : Bomb
{
    private void OnDestroy()
    {
        Explosion();
    }
}