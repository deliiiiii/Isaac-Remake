using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Block
{
    private void Awake()
    {
        size_x = size_y = 1;
        isBombable = true;
        isTearable = false;
        HP.Value = 1;
    }
}
