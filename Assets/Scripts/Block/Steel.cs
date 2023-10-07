using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steel : Block
{
    private void Awake()
    {
        size_x = size_y = 1;
        isBombable = false;
        isTearable = false;
        HP.Value = 1;
    }
}
