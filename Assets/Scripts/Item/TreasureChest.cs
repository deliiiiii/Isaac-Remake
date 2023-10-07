using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class TreasureChest : Item
{
    //public bool isInfinite = true;
    public bool isLocked;
    private bool isClosed = true;
    public float bounceForce = 8f;
    public Sprite sprite_locked;
    public Sprite sprite_unlocked;
    public List<Item> rewards_item = new();
    private void Awake()
    {
        value = 1;
        frictionSpeed = new(1f, 1f);
    }
    public void OpenChest()
    {
        if (!isClosed)
            return;
        for(int i=0;i<rewards_item.Count;i++)
        {
            Bounce(rewards_item[i]);
        }
        GetComponent<SpriteRenderer>().sprite = sprite_unlocked;
        isClosed = isLocked = false;
    }

    private void Bounce(Item obj)
    {
        obj = RoomManager.instance.currentRoom.Value.GenerateItem(gameObject.transform,obj.index,false);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //生成随机数 -1或1
        int random_x = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int random_y = UnityEngine.Random.Range(0, 2) * 2 - 1;
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(random_x*bounceForce, random_y * bounceForce), ForceMode2D.Impulse);
        obj.DelayCollect();
    }
}
