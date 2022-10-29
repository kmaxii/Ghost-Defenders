using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;

    private float _maxLives;


    // Start is called before the first frame update
    void Start()
    {
        _maxLives = pathFollower.lives;
    }

    // Update is called once per frame
    void Update()
    {
        float livePercent = pathFollower.lives / _maxLives;
        var newScale = transform.localScale;
        newScale = new Vector3(livePercent, newScale.y, newScale.z);
        transform.localScale = newScale;
    }
}