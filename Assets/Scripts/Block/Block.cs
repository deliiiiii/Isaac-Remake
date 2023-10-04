using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int index = -1;
    public int size_x = -1;
    public int size_y = -1;
    public int pos_x = -1;
    public int pos_y = -1;
    public bool? isBombable = null;
    public bool? isTearable = null;
    public ObservableValue<int> HP = new (-1, 7);
    public List<Sprite> sprite_HP = new();
    public void MDamage(int damage)
    {
        HP.Value -= damage;
    }
    public void DisableAllCollider()
    {
        if(GetComponent<BoxCollider2D>() != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if(GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
