using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fatty : Character
{
    protected override void Awake()
    {
        base.Awake();

        type = TYPE.enemy;
        index = 3;

        maxHP = new ObservableValue<int>(100, -1);
        curHP = new ObservableValue<int>(0, -1);
        curHP.Value = maxHP.Value;


        c_height = 1.1f;
        //character_Shade.transform.localPosition = new Vector3(0, -c_height, 0);
        moveSpeed = 1.8f;
        frictionSpeed = new Vector2(0.4f, 0.4f);
        tearDamage = 0;
        tearShootCD = 9999f;
        tearShootTimer = tearShootCD;
        tearSpeed = 0f;
        tearSpeedDivisionWhileMoving = 9999f;
        tearRange = 9999f;
        tearSize = 1f;


        skill_loadTimer = new() { 0.0f };
        skill_loadCD = new() { 0.0f };
        skill_usingMaxTime = new() { 0.0f };
        skill_usingTimer = new() { 0f };
        skill_range = new() { 9999f };
        skill_emit = new() { false };
        skillFuncs = new() { Skill_0 };


        skill_loadTimer.Add(1.5f);
        skill_loadCD.Add(2.0f);
        skill_usingMaxTime.Add(9999f);
        skill_usingTimer.Add(0f);
        skill_range.Add(2f);
        skill_emit.Add(false);
        skillFuncs.Add(StartSkill_1);
    }

    private void Update()
    {
        for (int i = 0; i < skill_loadTimer.Count; i++)
        {
            skill_loadTimer[i] += Time.deltaTime;
            if (skill_loadTimer[i] >= skill_loadCD[i] && !skill_emit[i])
            {
                if (i == 0 && skill_emit[1])
                    continue;
                skillFuncs[i]();
            }
        }
    }
    private void StartSkill_1()
    {
        skill_emit[1] = true;
        UsingSkill_1();
    }
    private void UsingSkill_1()
    {
        anim.SetTrigger("Emit");
    }
    private void SetAnim_Skill_1_Procedure_1()
    {
        skill_emit[1] = false;
        skill_usingTimer[1] = 0;
        skill_loadTimer[1] = 0;
    }
}
