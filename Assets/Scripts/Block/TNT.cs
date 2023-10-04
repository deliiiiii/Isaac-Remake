using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : Block
{
    public GameObject explosion;
    private float explodeTime = 0.15f;
    private void Awake()
    {
        size_x = size_y = 1;
        isBombable = true;
        isTearable = true;
        HP.Value = 3;
    }
    //public void SetAnim_before_Explode()
    //{
    //    GetComponent<Animator>().SetTrigger("Explode");
    //}
    public void SetAnim_after_Explode()
    {
        explosion.SetActive(true);
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(MyDestroy), explodeTime);
    }
    private void MyDestroy()
    {
        Destroy(explosion);
    }
}
