using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPathFollower : MonoBehaviour
{

    [SerializeField] private PathFollower pathFollower;

    private Vector2Int goingDirection = Vector2Int.zero;

    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
        
        if (rotationProgress < 1 && rotationProgress >= 0){
            rotationProgress += Time.deltaTime * 5;

            // Assign the interpolated rotation to transform.rotation
            // It will range from startRotation (rotationProgress == 0) to endRotation (rotationProgress >= 1)
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
        
    }

    private void UpdateDirection()
    {
        if (goingDirection != Vector2Int.zero && goingDirection == pathFollower.movingDirection)
            return;

        goingDirection = pathFollower.movingDirection;
        
        
        float rotZ = Mathf.Atan2(goingDirection.y, goingDirection.x) * Mathf.Rad2Deg;
        endRotation = Quaternion.Euler(0f, 0f, rotZ - 90);

        startRotation = transform.rotation;

        rotationProgress = 0;
        
        
    }
}
