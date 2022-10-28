using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteEvent : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Sprite changedSprite;

    public Sprite ActiveSprite
    {
        get => _spriteRenderer.sprite;
        set => _spriteRenderer.sprite = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
}