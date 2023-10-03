using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penny : Item
{
    private void Awake()
    {
        value = 1;
        frictionSpeed = new(0.75f, 0.75f);
    }
}
