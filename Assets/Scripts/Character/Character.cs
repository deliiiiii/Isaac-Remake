using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private int curEnemy = 0;

    public int index;
    public ObservableValue<int> curHP;
    public ObservableValue<int> maxHP;
    public ObservableValue<int> tempHP;
    public ObservableValue<int> blackHP;

    public float hurtTimer = 0;
    public float hurtCD = 1f;
    public bool isHurt = false;

    public float c_height;
    public float moveSpeed;
    public Vector2 frictionSpeed;
    public int tearDamage;
    public float tearShootCD;
    public float tearShootTimer;
    public float tearSpeed;
    public float tearSpeedDivisionWhileMoving;
    public float tearRange;
    public float tearSize;

    /// <summary>
    /// Enemy Skill
    /// </summary>
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
    protected delegate void SkillFuncs();
    protected List<SkillFuncs> skillFuncs;
    protected Vector3 target;

    public GameObject character_Shade;
    public Tear tear;
    public Tear purpleTear;

    protected Rigidbody2D rb;
    protected Animator anim;
    public enum STATE
    {
        Idling,
        Moving,
        ChangingRoom,
        Dead
    }
    public enum TYPE
    {
        player,
        enemy
    }
    public ObservableValue<STATE> currentState;
    public TYPE type;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentState = new(STATE.Idling,6);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy TO Player");
            collision.gameObject.GetComponent<Character>().MDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (type != TYPE.player)
            return;
        if(collision.gameObject.CompareTag("Item"))
        {

            if (!collision.gameObject.GetComponent<Item>().canCollect)
                return;
            int index_colliding = collision.gameObject.GetComponent<Item>().index;
            Debug.Log("colliding Item : " + index_colliding);
            if(collision.gameObject.GetComponent<Item>().index < 3)
                ItemManager.instance.prefab_item[index_colliding].count.Value += collision.gameObject.GetComponent<Item>().value;
            else if (index_colliding >= 3 && index_colliding <= 4)
            {
                if (curHP.Value == maxHP.Value)
                    return;
                if(index_colliding == 3)
                {
                    if (curHP.Value + 2 <= maxHP.Value)
                        curHP.Value += 2;
                    else
                        curHP.Value = maxHP.Value;
                }
                else if (index_colliding == 4)
                {
                    if (curHP.Value + 1 <= maxHP.Value)
                        curHP.Value += 1;
                    else
                        curHP.Value = maxHP.Value;
                }
            }
            else if (index_colliding == 8)
            {
                tear = purpleTear;
            }
            else if(index_colliding == 9)
            {
                maxHP.Value += 2;
                moveSpeed += 0.8f;
                tearShootCD -= 0.4f;
                tearDamage += 1;
            }
            else if(index_colliding == 10)
            {
                PlayerManager.instance.SetPlayer(1);
            }
            RoomManager.instance.currentRoom.Value.RemoveItem(collision.gameObject.GetComponent<Item>());
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Block"))
        {
            if(!collision.gameObject.GetComponent<TreasureChest>())
                return;
            if (ItemManager.instance.prefab_item[2].count.Value <= 0)
                return;
            if (!collision.gameObject.GetComponent<TreasureChest>().isLocked)
                ItemManager.instance.prefab_item[2].count.Value--;
            Debug.Log("Player TO TreasureChest");
            collision.gameObject.GetComponent<TreasureChest>().OpenChest();
        }


        if (collision.gameObject.CompareTag("Door") && currentState.Value != STATE.ChangingRoom)
        {
            if(collision.gameObject.GetComponent<Door>().base_state == Door.STATE.closed_1_Lock)
            {
                if (ItemManager.instance.prefab_item[2].count.Value <= 0)
                    return;
                ItemManager.instance.prefab_item[2].count.Value--;
                collision.gameObject.GetComponent<Door>().SetBaseState(Door.STATE.openNormal,false);
            }
            currentState.Value = STATE.ChangingRoom;
            int dir = collision.transform.GetSiblingIndex();
            transform.position = RoomManager.instance.MoveRoom(dir);
        }
    }

    public virtual void InputMove()
    {
        if(currentState.Value == STATE.Dead || currentState.Value == STATE.ChangingRoom)
        {
            return;
        }
        if (!type.Equals(TYPE.player))
            return;
        if(Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
            currentState.Value = STATE.Moving;
            SetAnim_Move("Move_W");
        }
        if(Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x,-moveSpeed);
            currentState.Value = STATE.Moving;
            SetAnim_Move("Move_S");
        }

        if(Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            currentState.Value = STATE.Moving;
            SetAnim_Move("Move_A");
        }
        if(Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            currentState.Value = STATE.Moving;
            SetAnim_Move("Move_D");
        }

        if(currentState.Value == STATE.Moving)
        {
            if(rb.velocity != Vector2.zero) 
            {
                FrictionSlowDown();
            }
            else
            {
                currentState.Value = STATE.Idling;
                SetAnim_Move(null);
                anim.SetTrigger("Idle");
            }
        }
    }
    public void SetAnim_Move(string name)
    {
        anim.SetBool("Move_W", false);
        anim.SetBool("Move_S", false);
        anim.SetBool("Move_A", false);
        anim.SetBool("Move_D", false);
        if (name != null)
            anim.SetBool(name, true);
    }
    public virtual void InputShoot()
    {
        if (currentState.Value == STATE.Dead || currentState.Value == STATE.ChangingRoom)
        {
            return;
        }
        if (!type.Equals(TYPE.player))
            return;
        if (tearShootTimer < tearShootCD)
        {
            tearShootTimer += Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            tearShootTimer = 0;
            tear.GenerateTear(gameObject, 1);
            //transform.position.y-c_height /tearSize
            return;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            tearShootTimer = 0;
            tear.GenerateTear(gameObject, 2);
            //transform.position.y-c_height /tearSize
            return;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            tearShootTimer = 0;
            tear.GenerateTear(gameObject,4);
            //transform.position.y-c_height /tearSize
            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            tearShootTimer = 0;
            tear.GenerateTear(gameObject, 3);
            //transform.position.y-c_height /tearSize
            return;
        }
    }
    public virtual void InputSkills()
    {
        if (currentState.Value == STATE.Dead || currentState.Value == STATE.ChangingRoom)
        {
            return;
        }
        if (!type.Equals(TYPE.player))
            return;
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            int count = 1;
            while (count > 0)
            {
                if (ItemManager.instance.prefab_item[1].count.Value <= 0)
                    return;
                ItemManager.instance.prefab_item[1].count.Value--;
                RoomManager.instance.currentRoom.Value.GenerateItem(transform, 5, false);
                count--;
            }
                
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            curEnemy++;
            if (curEnemy == EnemyManager.instance.prefab_enemy.Count)
                curEnemy = 0;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int count = 1;
            while(count > 0)
            {
                RoomManager.instance.currentRoom.Value.GenerateEnemy(RoomManager.instance.currentRoom.Value.gameObject.transform, curEnemy);
                count--;
            }
        }
    }

    public virtual void EmitSkill() 
    {
        if (!gameObject.CompareTag("Enemy"))
            return;
        for (int i = 0; i < skill_loadTimer.Count; i++)
        {
            skill_loadTimer[i] += Time.deltaTime;
            if (skill_loadTimer[i] >= skill_loadCD[i] && !skill_emit[i])
            {
                skillFuncs[i]();
            }
        }
    }
    
    private void FrictionSlowDown()
    {
        float t_x = rb.velocity.x, t_y = rb.velocity.y;
        if (t_x > 0)
        {
            t_x = t_x - frictionSpeed.x > 0 ? t_x - frictionSpeed.x : 0;
        }
        else
        {
            t_x = t_x + frictionSpeed.x < 0 ? t_x + frictionSpeed.x : 0;
        }
        if (t_y > 0)
        {
            t_y = t_y - frictionSpeed.y > 0 ? t_y - frictionSpeed.y : 0;
        }
        else
        {
            t_y = t_y + frictionSpeed.y < 0 ? t_y + frictionSpeed.y : 0;
        }
        rb.velocity = new Vector2(t_x, t_y);
    }

    public void MDamage(int damage)
    {
        if (isHurt)
            return;
        
        curHP.Value -= damage;
        if (gameObject.CompareTag("Enemy"))
        {
            if (curHP.Value <= 0)
            {
                RoomManager.instance.currentRoom.Value.RemoveEnemy(GetComponent<Character>());
                Destroy(gameObject);
            }
        }
        //Debug.Log("new HP = " + curHP.Value);
        if (!gameObject.CompareTag("Player"))
            return;
        isHurt = true;
        StartCoroutine(nameof(HurtFlash));
    }
    IEnumerator HurtFlash()
    {
        int count = 0;
        while(count < 100)
        {
            hurtTimer += 0.08f;
            if (hurtTimer > hurtCD)
            {
                hurtTimer = 0;
                isHurt = false;
                break;
            }
            ReverseSpriteActive();
            yield return new WaitForSeconds(0.08f);
            count++;
        }
        for (int i = 0; i < transform.childCount - 1; i++)//PAT:the last is character's shade
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
        }
        yield break;

    }
    private void ReverseSpriteActive()
    {
        for(int i = 0;i<transform.childCount-1;i++)//PAT:the last is character's shade
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = !transform.GetChild(i).GetComponent<SpriteRenderer>().enabled;
        }
    }

    public void Skill_0()
    {
        anim.SetBool("Move", true);
        anim.SetBool("Idle", false);
        Character player = GameManager.instance.player;
        if (CheckNear(player.transform.position, transform.position, skill_range[0]))
        {
            skill_emit[0] = true;
            //Debug.Log("Next to player : MOVE!!");
            Vector3 delta_vec = (player.transform.position - transform.position).normalized;
            transform.GetComponent<Rigidbody2D>().velocity = delta_vec * moveSpeed;
            target = player.transform.position;
            EndSkill_0();
            //StartCoroutine(nameof(EndSkill_1),player.transform);
        }
        else
        {
            skill_emit[0] = true;
            //Debug.Log("# Random MOVE!!");
            Vector3 delta_vec = new(Random.Range(-1.2f, 1.2f), UnityEngine.Random.Range(-1.2f, 1.2f), 0f);
            transform.GetComponent<Rigidbody2D>().velocity = delta_vec * moveSpeed;
            target = new(1e9f, 1e9f, 0f);
            //StartCoroutine(nameof(EndSkill_1), remoteDirection);
            EndSkill_0();
        }
    }
    public void EndSkill_0()
    {
        //anim.SetBool("Idle", true);
        //anim.SetBool("Move", false);
        if (!CheckNear(transform.position, target, 0.1f) && skill_usingTimer[0] <= skill_usingMaxTime[0])
        {
            skill_usingTimer[0] += 0.02f;
            Invoke(nameof(EndSkill_0), 0.02f);
            return;
        }
        skill_emit[0] = false;
        skill_usingTimer[0] = 0;
        skill_loadTimer[0] = 0;
        //Invoke(nameof(StopMove), slidingTime);
    }
    public bool CheckNear(Vector3 pos1, Vector3 pos2, float f_distance)
    {
        float distance = Vector3.Distance(pos1, pos2);
        if (distance > f_distance)
            return false;
        return true;
    }
}
