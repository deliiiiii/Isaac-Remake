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
        RoomManager.instance.AddRoomToList();
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
            Debug.Log("C");
            if (CheckNear(transform.position,target))
            {
                transform.position = new Vector3(target.x, target.y, transform.position.z);
                Debug.Log("B1");
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
                Debug.Log("B2");
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
