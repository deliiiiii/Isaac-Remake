using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMask : MonoBehaviour
{
    void Update()
    {
        GetComponent<SpriteMask>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
}
