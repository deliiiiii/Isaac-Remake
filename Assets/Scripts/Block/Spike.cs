using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Block
{
    private void Awake()
    {
        size_x = size_y = 1;
        isBombable = false;
        isTearable = false;
        HP.Value = 1;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Enemy TO Player");
            collision.gameObject.GetComponent<Character>().MDamage(1);
        }
    }
}
