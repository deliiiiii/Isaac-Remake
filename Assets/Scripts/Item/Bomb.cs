using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    private void Awake()
    {
        value = 1;
        frictionSpeed = new(0.82f, 0.82f);
    }
}
