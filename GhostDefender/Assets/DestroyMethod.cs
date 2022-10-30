using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyMethod : MonoBehaviour
{

    private TextMeshPro _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }


    public void Destroy()
    {
        transform.parent.SetParent(null);
        DamageIndicator.Return(_textMeshPro);
    }
}
