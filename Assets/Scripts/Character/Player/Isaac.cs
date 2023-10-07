using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isaac : Character
{
    private void Start()
    {
        type = TYPE.player;
        index = 0;

        maxHP = new ObservableValue<int>(14, 1);
        curHP = new ObservableValue<int>(0, 1);
        tempHP = new ObservableValue<int>(0, 1);
        blackHP = new ObservableValue<int>(0, 1);
        curHP.Value = maxHP.Value;
        tempHP.Value = 0;
        blackHP.Value = 0;

        c_height = 0.7f;
        character_Shade.transform.localPosition = new Vector3(0,-c_height,0);
        moveSpeed = 5.0f;
        frictionSpeed = new Vector2 (0.4f, 0.4f);
        tearDamage = 6;
        tearShootCD = 0.5f;
        tearShootTimer = tearShootCD;
        tearSpeed = 6.5f;
        tearSpeedDivisionWhileMoving = 4.5f;
        tearRange = 4.0f;
        tearSize = 0.7f;
    }
    private void FixedUpdate()
    {
        base.InputMove();
    }
    private void Update()
    {
        base.InputShoot();
        base.InputSkills();

        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ItemManager.instance.prefab_item[0].count.Value += 20;
        //    ItemManager.instance.prefab_item[1].count.Value += 20;
        //    ItemManager.instance.prefab_item[2].count.Value += 20;
        //}
    }
    //public override void InputMove()
    //{
    //    base.InputMove();
    //}

    
}
