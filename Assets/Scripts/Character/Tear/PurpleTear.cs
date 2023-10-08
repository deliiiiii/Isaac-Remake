using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleTear : Tear
{
    public float subVelovityDivision = 10f;
    private GameObject target = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && target == null)
        {
            target = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && target == collision.gameObject)
        {
            target = null;
        }
    }
    private void Update()
    {
        if (target != null && !isDestroyed)
        {
            Vector3 delta = (target.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = 
                (GetComponent<Rigidbody2D>().velocity.normalized + 
                (Vector2)delta / subVelovityDivision).normalized*user.tearSpeed;
        }
    }
}
