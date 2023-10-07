using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : Block
{
    private bool isUsed = false;
    private void Awake()
    {
        size_x = size_y = 1;
        isBombable = isTearable = true;
        HP.Value = 3;
    }
    private void Update()
    {
        if(HP.Value <= 0 && !isUsed)
        {
            isUsed = true;
            HP.Value = 0;
            int ran = UnityEngine.Random.Range(0, 10001);
            if(ran <= 200)
            {
                RoomManager.instance.currentRoom.Value.GenerateItem(transform, UnityEngine.Random.Range(0, 8),false);
            }
        }
    }
}
