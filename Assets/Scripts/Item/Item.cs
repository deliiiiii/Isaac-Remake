using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ObservableValue<int> count = new (-1, 2);
    public int index = -1;
    public int value = -1;
    public bool canCollect = true;
    protected Vector2 frictionSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!rb)
            return;
        if (rb.velocity != Vector2.zero)
        {
            FrictionSlowDown();
        }
    }
    private void FrictionSlowDown()
    {
        float t_x = rb.velocity.x, t_y = rb.velocity.y;
        if (t_x > 0)
        {

            t_x = t_x - frictionSpeed.x > 0 ? t_x - frictionSpeed.x : 0;
        }
        else
        {
            t_x = t_x + frictionSpeed.x < 0 ? t_x + frictionSpeed.x : 0;
        }
        if (t_y > 0)
        {
            t_y = t_y - frictionSpeed.y > 0 ? t_y - frictionSpeed.y : 0;
        }
        else
        {
            t_y = t_y + frictionSpeed.y < 0 ? t_y + frictionSpeed.y : 0;
        }
        rb.velocity = new Vector2(t_x, t_y);
    }

    public void DelayCollect()
    {
        canCollect = false;
        Invoke(nameof(EnableCollect), 1f);
    }
    private void EnableCollect()
    {
        canCollect = true;
    }
}
