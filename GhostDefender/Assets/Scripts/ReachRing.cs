using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachRing : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void MakeRed()
    {
        _spriteRenderer.color = new Color(255, 0, 0, 0.454f);
    }

    public void MakeBlack()
    {
        _spriteRenderer.color = new Color(0, 0, 0, 0.454f);
    }

    public void ShowRing(float boxColliderRadius)
    {
        ShowRing();
        boxColliderRadius *= 2;
        transform.localScale = new Vector3(boxColliderRadius, boxColliderRadius, boxColliderRadius);
    }

    public void ShowRing()
    {
        _spriteRenderer.enabled = true;

    }

    public void HideRing()
    {
        _spriteRenderer.enabled = false;
    }
}
