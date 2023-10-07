using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emit : MonoBehaviour
{
    private float explodeForce = 10f;
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 dir = collision.gameObject.transform.position - transform.position;
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Emit TO Player");
            
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explodeForce, ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Emit TO Block");

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explodeForce/1000000, ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject !=gameObject.transform.parent.gameObject)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explodeForce, ForceMode2D.Impulse);
        }
    }

}
