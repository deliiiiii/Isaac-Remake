using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explodeForce = 0.4f;
    private int damageToEnemy = 35;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Block"))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if (block)
            {
                if ((bool)block.isBombable)
                {
                    block.MDamage(10);
                }
            }
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            player.MDamage(1);
            Vector2 dir = collision.gameObject.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explodeForce, ForceMode2D.Impulse);
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bomb TO Enemy");
            collision.gameObject.GetComponent<Character>().MDamage(damageToEnemy);
            Vector2 dir = collision.gameObject.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explodeForce, ForceMode2D.Impulse);
        }
        if(collision.gameObject.CompareTag("Item"))
        {
            Vector2 dir = collision.gameObject.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explodeForce, ForceMode2D.Impulse);
        }
    }
}
