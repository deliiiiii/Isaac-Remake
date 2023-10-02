using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    public float height;
    public float rangeCount;

    public float fallingSpeed;
    public float fallingAcceleration;

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
        //tear_Shade.transform.position = new Vector3(0f, -height / user.tearSize, 0f);
    }
    private void FixedUpdate()
    {
        if(!gameObject.activeSelf)
            return;

        if(rangeCount < user.tearRange)
            rangeCount += user.tearSpeed*Time.deltaTime;
        else
        {
            fallingSpeed += fallingAcceleration * Time.deltaTime;
            transform.position = new Vector3 (transform.position.x, transform.position.y - fallingSpeed*Time.deltaTime, transform.position.z);
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y + fallingSpeed * Time.deltaTime, transform.GetChild(0).position.z);
            if (transform.position.y < transform.GetChild(0).position.y + 0.1f)
                Destroy(gameObject);
        }
    }
    public void GenerateTear(GameObject user,int direction)
    {
        this.user = user.GetComponent<Character>();
        int[,] dir = new int[2, 5]
        {
            {0,0,0,-1,1},
            {0,1,-1,0,0}
        };
        GameObject gene_tear = Instantiate(gameObject, new Vector3(user.transform.position.x + dir[0,direction], user.transform.position.y + dir[1, direction], transform.position.z), Quaternion.identity);
        gene_tear.SetActive(true);
        gene_tear.transform.localScale = new Vector3(user.GetComponent<Character>().tearSize, user.GetComponent<Character>().tearSize, 1f);
        gene_tear.GetComponent<Tear>().tear_Shade.transform.localPosition = new Vector3
            (
            0f,
            -user.GetComponent<Character>().c_height / user.GetComponent<Character>().tearSize,
            0f
            );
        gene_tear.GetComponent<Rigidbody2D>().velocity = new Vector2
            (
            user.GetComponent<Rigidbody2D>().velocity.x/ user.GetComponent<Character>().tearSpeedDivisionWhileMoving + dir[0, direction] * user.GetComponent<Character>().tearSpeed,
            user.GetComponent<Rigidbody2D>().velocity.y/ user.GetComponent<Character>().tearSpeedDivisionWhileMoving + dir[1, direction] * user.GetComponent<Character>().tearSpeed
            );
        //Debug.Log("ultimate Speed.x = " + (user.GetComponent<Rigidbody2D>().velocity.x / user.GetComponent<Character>().tearSpeedDivisionWhileMoving + dir[0, direction] * user.GetComponent<Character>().tearSpeed));

        
    }
}
