using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
  

    public void ChangeToRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
