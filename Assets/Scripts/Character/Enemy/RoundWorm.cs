using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHead : Character
{
    private int skill_1_ShotMax = 1;
    private int skill_1_ShotCount = 0;
    protected override void Awake()
    {
        base.Awake();

        type = TYPE.enemy;
        index = 2;

        maxHP = 20;
        curHP = new ObservableValue<int>(0, -1);
        curHP.Value = maxHP;


        c_height = 0.6f; 
        moveSpeed = 0f;
        frictionSpeed = new Vector2(9999f, 9999f);
        tearDamage = 1;
        tearSpeed = 5f;
        tearSpeedDivisionWhileMoving = 0;
        tearRange = 7.5f;
        tearSize = 0.7f;

        skill_loadTimer = new() { 0f };
        skill_loadCD = new() { 9999f };
        skill_usingMaxTime = new() { 9999f };
        skill_usingTimer = new() { 0f };
        skill_range = new() { 0f };
        skill_emit = new() { false };
        skillFuncs = new() { Skill_0 };

        skill_loadTimer.Add(3f);
        skill_loadCD.Add(4f);
        skill_usingMaxTime.Add(1.2f);
        skill_usingTimer.Add(0f);
        skill_range.Add(4f);
        skill_emit.Add(false);
        skillFuncs.Add(delegate { StartSkill_1(); });
    }

    private void Update()
    {
        base.EmitSkill();
    }
    private void StartSkill_1()
    {
        skill_emit[1] = true;
        UsingSkill_1();
    }

    private void UsingSkill_1()
    {
        anim.SetTrigger("Appear");
    }
    public void SetAnim_Skill_1_Procedure_1()
    {
        Debug.Log("SetAnim_Skill_1_Procedure_1");
        if (CheckNear(transform.position, GameManager.instance.player.transform.position, skill_range[1]) && skill_1_ShotCount < skill_1_ShotMax)
        {
            skill_1_ShotCount++;
            Debug.Log("Next to player");
            tear.GenerateTear(gameObject, GameManager.instance.player.transform.position - transform.position);
        }
        if (skill_usingTimer[1] <= skill_usingMaxTime[1])
        {
            skill_usingTimer[1] += 0.02f;
            Invoke(nameof(SetAnim_Skill_1_Procedure_1), 0.02f);
            return;
        }
        anim.SetTrigger("Disappear");
    }
    public void SetAnim_Skill_1_Procedure_2()
    {
        skill_1_ShotCount = 0;
        skill_emit[1] = false;
        skill_usingTimer[1] = 0;
        skill_loadTimer[1] = 0;
    }
}
