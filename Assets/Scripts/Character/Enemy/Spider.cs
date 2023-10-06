using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Spider : Character
{
    [Tooltip("停止移动之后的滑动时间")]
    private float slidingTime = 0.35f;
    [Tooltip("技能CD")]
    public List<float> skill_loadCD;
    [Tooltip("技能充能条")]
    public List<float> skill_loadTimer;
    [Tooltip("技能阻回上限")]
    public List<float> skill_usingMaxTime;
    [Tooltip("技能阻回条")]
    public List<float> skill_usingTimer;
    [Tooltip("技能攻击范围")]
    public List<float> skill_range;
    [Tooltip("是否正在释放技能")]
    public List<bool> skill_emit;
    private delegate bool SkillFuncs(int skillIndex);
    private List<SkillFuncs> skillFuncs;

    private Vector3 target;
    private void Awake()
    {
        type = TYPE.enemy;
        index = 0;

        maxHP = 11;
        curHP = new ObservableValue<int>(0, -1);//TODO enemy HP
        curHP.Value = maxHP;


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

        
        skill_loadTimer = new() { 0.5f };
        skill_loadCD = new() { 0.8f };
        skill_usingMaxTime = new() { 0.8f };
        skill_usingTimer = new() { 0f };
        skill_range = new() { 3f };
        skill_emit = new() { false };
        skillFuncs = new() { MoveTowards };
    }

    private void Update()
    {
        for(int i = 0; i < skill_loadTimer.Count; i++)
        {
            skill_loadTimer[i] += Time.deltaTime;
            if (skill_loadTimer[i] >= skill_loadCD[i] && !skill_emit[i])
            {
                //Debug.Log("Emit Skill :" + i); 
                skillFuncs[i](i);
            }
        }
    }

    bool MoveTowards(int skillIndex)
    {
        Character player = GameManager.instance.player;
        if(CheckNear(player.transform.position,transform.position, skill_range[skillIndex]))
        {
            skill_emit[skillIndex] = true;
            //Debug.Log("Next to player : MOVE!!");
            Vector3 delta_vec = (player.transform.position - transform.position).normalized;
            transform.GetComponent<Rigidbody2D>().velocity = delta_vec*moveSpeed;
            target = player.transform.position;
            EndSkill_1();
            //StartCoroutine(nameof(EndSkill_1),player.transform);
        }
        else
        {
            skill_emit[skillIndex] = true;
            //Debug.Log("# Random MOVE!!");
            Vector3 delta_vec = new(Random.Range(-1.2f, 1.2f), UnityEngine.Random.Range(-1.2f, 1.2f), 0f);
            transform.GetComponent<Rigidbody2D>().velocity = delta_vec * moveSpeed;
            target = new (1e9f, 1e9f, 0f);
            //StartCoroutine(nameof(EndSkill_1), remoteDirection);
            EndSkill_1();
        }
        return true;
    }
    public void EndSkill_1()
    {
        if(!CheckNear(transform.position,target,0.1f) && skill_usingTimer[0] <= skill_usingMaxTime[0])
        {
            skill_usingTimer[0] += 0.02f;
            Invoke(nameof(EndSkill_1), 0.02f);
            return;
        }
        skill_emit[0] = false;
        skill_usingTimer[0] = 0;
        skill_loadTimer[0] = 0;
        Invoke(nameof(StopMove), slidingTime);
    }
    public void StopMove()
    {
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    //check the distance between pos1 and pos2
    bool CheckNear(Vector3 pos1, Vector3 pos2,float f_distance)
    {
        float distance = Vector3.Distance(pos1, pos2);
        if(distance > f_distance)
            return false;
        return true;
    }
}
