using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Spider : Character
{
    
    protected override void Awake()
    {
        base.Awake();

        type = TYPE.enemy;
        index = 0;

        maxHP = new ObservableValue<int>(11, -1);
        curHP = new ObservableValue<int>(0, -1);//TODO enemy HP
        curHP.Value = maxHP.Value;


        c_height = 0.3f;
        character_Shade.transform.localPosition = new Vector3(0, -c_height, 0);
        moveSpeed = 7.5f;
        frictionSpeed = new Vector2(0.4f, 0.4f);
        tearDamage = 0;
        tearShootCD = 9999f;
        tearShootTimer = tearShootCD;
        tearSpeed = 0f;
        tearSpeedDivisionWhileMoving = 9999f;
        tearRange = 9999f;
        tearSize = 1f;

        
        skill_loadTimer = new() { 0.25f };
        skill_loadCD = new() { 0.5f };
        skill_usingMaxTime = new() { 0.8f };
        skill_usingTimer = new() { 0f };
        skill_range = new() { 3.2f };
        skill_emit = new() { false };
        skillFuncs = new() { Skill_0 };
    }

    private void Update()
    {
        base.EmitSkill();
    }

    
    
    //public void StopMove()
    //{
    //    Debug.Log("Stop Move");
        //transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    //}
    //check the distance between pos1 and pos2
    
}
