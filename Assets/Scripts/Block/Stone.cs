using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Block
{
    private bool isUsed = false;
    private void Awake()
    {
        size_x = size_y = 1;
        isBombable = true;
        isTearable = false;
        HP.Value = 1;
    }
    private void Update()
    {
        if (HP.Value <= 0 && !isUsed)
        {
            isUsed = true;
            HP.Value = 0;
            int ran = UnityEngine.Random.Range(0, 10001);
            if (ran <= 250)
            {
                RoomManager.instance.currentRoom.Value.GenerateItem(transform, UnityEngine.Random.Range(0, 8), false);
            }
        }
    }
}
