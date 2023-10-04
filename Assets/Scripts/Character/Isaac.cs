using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isaac : Character
{
    private void Start()
    {
        type = TYPE.player;

        maxHP = 13;
        curHP = new ObservableValue<int>(0, 1);
        tempHP = new ObservableValue<int>(0, 1);
        blackHP = new ObservableValue<int>(0, 1);
        curHP.Value = maxHP;
        tempHP.Value = 0;
        blackHP.Value = 0;

        c_height = 0.7f;
        character_Shade.transform.localPosition = new Vector3(0,-c_height,0);
        moveSpeed = 5.0f;
        frictionSpeed = new Vector2 (0.4f, 0.4f);
        tearDamage = 3.5f;
        tearShootCD = 0.8f;
        tearShootTimer = tearShootCD;
        tearSpeed = 10f;
        tearSpeedDivisionWhileMoving = 4.5f;
        tearRange = 4.0f;
        tearSize = 0.62f;
    }
    private void FixedUpdate()
    {
        base.InputMove();
    }
    private void Update()
    {
        base.InputShoot();
        base.InputSkills();
    }
    //public override void InputMove()
    //{
    //    base.InputMove();
    //}

    
}
