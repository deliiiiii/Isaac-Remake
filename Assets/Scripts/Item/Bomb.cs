using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    public GameObject explosion;
    public LayerMask characterHead;
    public LayerMask characterBody;
    private float explodeTime = 0.15f;
    private void Awake()
    {
        value = 1;
        frictionSpeed = new(1.2f, 1.2f);
    }
    private void Update()
    {
        //if (!enabled)
            //return;
        //existTime -= Time.deltaTime;
        //if (existTime < 0)
            //SetAnim_before_Explode();
    }
    public void SetAnim_before_Explode()
    {
        GetComponent<CircleCollider2D>().excludeLayers += characterHead;
        GetComponent<CircleCollider2D>().excludeLayers += characterBody;
        GetComponent<Animator>().SetTrigger("Explode");
    }
    private void SetAnim_after_Explode()
    {
        explosion.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(MyDestroy),explodeTime);
    }
    private void MyDestroy()
    {
        Destroy(gameObject);
    }
}
