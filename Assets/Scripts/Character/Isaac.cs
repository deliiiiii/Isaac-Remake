using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isaac : Character
{

    private new void Start()
    {
        base.Start();
        currentState = STATE.Idling;
        type = TYPE.player;
        maxHP = 3.0f;
        curHP.Value = maxHP;
        tempHP.Value = 0;
        blackHP.Value = 0;

        c_height = 0.7f;
        character_Shade.transform.position = new Vector3(0,-c_height,0);
        moveSpeed = 5.0f;
        frictionSpeed = new Vector2 (0.4f, 0.4f);
        tearDamage = 3.5f;
        tearShootCD = 0.8f;
        tearShootTimer = tearShootCD;
        tearSpeed = 6.5f;
        tearSpeedDivisionWhileMoving = 4.5f;
        tearRange = 3.0f;
        tearSize = 0.3f;
    }
    private void FixedUpdate()
    {
        InputMove();
    }
    private void Update()
    {
        base.InputShoot();
    }
    public override void InputMove()
    {
        base.InputMove();
    }

    
}
