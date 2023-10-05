using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private float speed_ChangeRoom = 2.5f;
    public Vector3 target;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        RoomManager.instance.AddRoomToList();
        BlockManager.instance.ReadBlock();
        ItemManager.instance.ReadItem();
        UIManager.instance.AddTextToList();

        ItemManager.instance.prefab_item[0].count.Value = 0;
        ItemManager.instance.prefab_item[1].count.Value = 66;
        ItemManager.instance.prefab_item[2].count.Value = 2;

        //RoomManager.instance.currentRoom.Value.GenerateBlock(0, 5);
        //RoomManager.instance.currentRoom.Value.GenerateBlock(1, 3);
        //RoomManager.instance.currentRoom.Value.GenerateBlock(2, 20);
        RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 0,true);
        RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 1,true);
        RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 2,true);
    }
    public void CallRefreshPosition()
    {
        target = RoomManager.instance.currentRoom.Value.pos_world;
        StartCoroutine(nameof(RefreshPosition));
    }
    public IEnumerator RefreshPosition()
    {
        while (true) 
        {
            //Debug.Log("C");
            if (CheckNear(transform.position,target))
            {
                transform.position = new Vector3(target.x, target.y, transform.position.z);
                //Debug.Log("B1");
                break;
            }
            Vector3 iden_delta = (target - transform.position).normalized;
            transform.position = new Vector3
                (transform.position.x + iden_delta.x * speed_ChangeRoom,
                transform.position.y + iden_delta.y * speed_ChangeRoom,
                transform.position.z);
            Vector3 iden_delta_after = (target - transform.position).normalized;
            if (iden_delta.x * iden_delta_after.x < 0 || iden_delta.y * iden_delta_after.y < 0)
            {
                //Debug.Log("B2");
                transform.position = new Vector3(target.x,target.y,transform.position.z);
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        RoomManager.instance.CheckRoomState();
        yield break;
    }
    private bool CheckNear(Vector3 v1,Vector3 v2) 
    {
        if (Math.Abs(v1.x - v2.x) <= 0.08f && Math.Abs(v1.y - v2.y) <= 0.08f)
            return true;
        return false;
    }
}
