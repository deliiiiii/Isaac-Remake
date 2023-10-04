using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HP : Item
{
    public string type;
    private void Awake()
    {
        frictionSpeed = new(1.0f, 1.0f);
    }
}
