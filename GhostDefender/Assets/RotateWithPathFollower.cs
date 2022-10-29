using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPathFollower : MonoBehaviour
{

    [SerializeField] private PathFollower pathFollower;

    private Vector2Int goingDirection = Vector2Int.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        if (goingDirection != Vector2Int.zero && goingDirection == pathFollower.movingDirection)
            return;

        goingDirection = pathFollower.movingDirection;
        
        
        float rotZ = Mathf.Atan2(goingDirection.y, goingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        
    }
}
