using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    public float height;
    public float rangeCount;

    public float fallingSpeed;
    public float fallingAcceleration;
    public bool isDestroyed;
    public Character user;
    public GameObject tear_Shade;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rangeCount = 0;
        fallingSpeed = 0f;
        fallingAcceleration = 9.8f;
        isDestroyed = false;
        //tear_Shade.transform.position = new Vector3(0f, -height / user.tearSize, 0f);
    }
    private void FixedUpdate()
    {
        if(!gameObject.activeSelf || isDestroyed)
            return;

        if(rangeCount < user.tearRange)
            rangeCount += user.tearSpeed*Time.deltaTime;
        else
        {
            fallingSpeed += fallingAcceleration * Time.deltaTime;
            transform.position = new Vector3 (transform.position.x, transform.position.y - fallingSpeed*Time.deltaTime, transform.position.z);
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y + fallingSpeed * Time.deltaTime, transform.GetChild(0).position.z);
            if (transform.position.y < transform.GetChild(0).position.y + 0.1f)
                Anim_before_Destroy();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if(block)
            {
                if((bool)block.isTearable)
                {
                    block.MDamage(1);
                }
            }
            Anim_before_Destroy();
        }
        if (collision.gameObject.CompareTag("Item"))
        {
            Anim_before_Destroy();
        }

        if (user.type == Character.TYPE.player && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Character>().MDamage(user.tearDamage);
            //if (collision.gameObject.GetComponent<Character>().curHP.Value <= 0)
            //{
            //    RoomManager.instance.currentRoom.Value.RemoveEnemy(collision.gameObject.GetComponent<Character>());
            //    Destroy(collision.gameObject);
            //}
            Anim_before_Destroy();
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().MDamage(1); 
            Anim_before_Destroy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))//PAT as well as Emit(Fatty's skill)
        {
            Anim_before_Destroy();
        }
    }
    public void GenerateTear(GameObject user,int direction)
    {
        this.user = user.GetComponent<Character>();
        float[,] dir = new float[3, 5]
        {
            {0,0,0,-0.6f,0.6f},
            {0,0.6f,-0.6f,0,0},
            {0,0.6f,-0.6f,-0.2f,-0.2f}//·ÀÖ¹×Óµ¯ÌùÇ½·¢ÉäµÄÎ»ÒÆÐÞ¸´
        };
        GameObject gene_tear = Instantiate(gameObject, new Vector3(user.transform.position.x + dir[0,direction], user.transform.position.y + dir[2, direction], transform.position.z), Quaternion.identity);
        gene_tear.SetActive(true);
        gene_tear.transform.localScale = new Vector3(user.GetComponent<Character>().tearSize, user.GetComponent<Character>().tearSize, 1f);
        gene_tear.GetComponent<Tear>().tear_Shade.transform.localPosition = new Vector3
            (
            0f,
            -user.GetComponent<Character>().c_height / user.GetComponent<Character>().tearSize,
            0f
            );
        gene_tear.GetComponent<Tear>().tear_Shade.transform.localScale = new Vector3(0.5f,0.2f,1f);
        gene_tear.GetComponent<Rigidbody2D>().velocity = new Vector2
            (
            user.GetComponent<Rigidbody2D>().velocity.x/ user.GetComponent<Character>().tearSpeedDivisionWhileMoving + dir[0, direction] * user.GetComponent<Character>().tearSpeed,
            user.GetComponent<Rigidbody2D>().velocity.y/ user.GetComponent<Character>().tearSpeedDivisionWhileMoving + dir[1, direction] * user.GetComponent<Character>().tearSpeed
            );
    }
    public void GenerateTear(GameObject user, Vector3 direction)
    {
        this.user = user.GetComponent<Character>();
        GameObject gene_tear = Instantiate(gameObject, direction.normalized/2+user.transform.position, Quaternion.identity);
        gene_tear.SetActive(true);
        gene_tear.transform.localScale = new Vector3(user.GetComponent<Character>().tearSize, user.GetComponent<Character>().tearSize, 1f);
        gene_tear.GetComponent<Tear>().tear_Shade.transform.localPosition = new Vector3
            (
            0f,
            -user.GetComponent<Character>().c_height / user.GetComponent<Character>().tearSize,
            0f
            );
        gene_tear.GetComponent<Tear>().tear_Shade.transform.localScale = new Vector3(0.5f, 0.2f, 1f);
        gene_tear.GetComponent<Rigidbody2D>().velocity =(Vector2) direction.normalized * user.GetComponent<Character>().tearSpeed;
    }
    void Anim_before_Destroy()
    {
        isDestroyed = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetTrigger("OnDestroy");
        GetComponent<CircleCollider2D>().enabled = false;
    }
    void Anim_after_Destroy() 
    {
        Destroy(gameObject);
    }
}
