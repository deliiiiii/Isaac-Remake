using UnityEngine;
using UnityEngine.XR;

public class Character : MonoBehaviour
{
    protected ObservableValue<float> curHP;
    protected float maxHP;
    protected ObservableValue<float> tempHP;
    protected ObservableValue<float> blackHP;

    public float c_height;
    public float moveSpeed;
    public Vector2 frictionSpeed;
    public float tearDamage;
    public float tearShootCD;
    public float tearShootTimer;
    public float tearSpeed;
    public float tearSpeedDivisionWhileMoving;
    public float tearRange;
    public float tearSize;
    

    public GameObject character_Shade;
    public Tear tear;

    private Rigidbody2D rb;
    private Animator anim;
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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentState = new(STATE.Idling,6);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Item"))
        {
            Debug.Log("colliding Item : " + collision.gameObject.GetComponent<Item>().index);
            ItemManager.instance.prefab_item[collision.gameObject.GetComponent<Item>().index].count.Value += collision.gameObject.GetComponent<Item>().value;
            Destroy(collision.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door") && currentState.Value != STATE.ChangingRoom)
        {
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
    public virtual void InputShoot()
    {
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

    public void SetAnim_Move(string name)
    {
        anim.SetBool("Move_W", false);
        anim.SetBool("Move_S", false);
        anim.SetBool("Move_A", false);
        anim.SetBool("Move_D", false);
        if (name != null)
            anim.SetBool(name, true);
    }

    public void RefreshHPUI()
    {
        Debug.Log("RefreshHPUI");
    }
}
