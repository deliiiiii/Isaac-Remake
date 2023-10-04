using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public List<Room> list_room = new();
    public ObservableValue<Room> currentRoom = new(new(),4);
    private int width = 3;
    private int column = 3;


    int[,] dir_or_index_door = new int[3, 5]
    {
        {0,0,0,-1,1 },
        {0,1,-1,0,0 },
        {0,2,1,4,3 },//新房间门的序号
    };

    public GameObject grid;
    public GameObject room_default;


    private void Awake()
    {
        instance = this;
    }
    public void AddRoomToList()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < column; j++)
            {
                list_room.Add(Instantiate(room_default, new Vector3(i * 35, j * 24, 0), Quaternion.identity, grid.transform).GetComponent<Room>());
                list_room[^1].pos_x = i + 1;
                list_room[^1].pos_y = j + 1;
                list_room[^1].gameObject.SetActive(true);   
            }
        currentRoom.Value = list_room[0];
        CameraController.instance.CallRefreshPosition();
        RefreshDoorState();
    }
    public void RefreshDoorState()
    {
        for (int i = 0;i<list_room.Count;i++)
        {
            for (int direction = 1;direction<=4;direction++)
            {
                Room hasFoundRoom = FindRoom(list_room[i].pos_x + dir_or_index_door[0, direction], list_room[i].pos_y + dir_or_index_door[1, direction]);
                //Debug.Log("RoomIndex:" + i + " direction : " + direction +" found? :" + hasFoundRoom);
                if (hasFoundRoom != null)
                {
                    list_room[i].doors[direction].SetActive(true);
                }
                else
                    list_room[i].doors[direction].SetActive(false);
            }
        }
    }
    public void RefreshBlockState()
    {
        for(int i=0;i<currentRoom.Value.blocks.Count;i++)
        {
            Block block = currentRoom.Value.blocks[i];
            int HP = block.HP.Value;
            //Debug.Log("HP = " + HP);
            if(HP <= 0)
            {
                block.DisableAllCollider();
                currentRoom.Value.blocks.RemoveAt(i);
                block.GetComponent<SpriteRenderer>().sprite = block.sprite_HP[0];

                if(block.index == 2)
                {
                    block.GetComponent<TNT>().SetAnim_after_Explode();
                }
                continue;
            }
            block.GetComponent<SpriteRenderer>().sprite = block.sprite_HP[HP];
        }
    }
    public Vector3 MoveRoom(int direction)
    {
        
        float[,] delta_makeup = new float[2, 5]
        {
            {0,0,0,-0.9f,0.9f },
            {0,1.4f,-0.5f,0.4f,0.4f},
        };
        //Debug.Log("current x = " + currentRoom.Value.pos_x);
        //Debug.Log("current y = " + currentRoom.Value.pos_y);
        //Debug.Log("direction = " + direction);
        //Debug.Log("dir x = " + dir_or_index_door[0,direction]);
        //Debug.Log("dir y = " + dir_or_index_door[1,direction]);
        Room targetRoom = FindRoom(currentRoom.Value.pos_x + dir_or_index_door[0, direction], currentRoom.Value.pos_y + dir_or_index_door[1, direction]);
        currentRoom.Value = targetRoom;
        Vector3 targetPos = new
            (targetRoom.doors[dir_or_index_door[2, direction]].transform.position.x + delta_makeup[0, direction],
            targetRoom.doors[dir_or_index_door[2, direction]].transform.position.y + delta_makeup[1, direction],
            0);
        return targetPos;
    }
    public Room FindRoom(int x,int y)
    {
        for(int i = 0;i<list_room.Count;i++)
        {
            if (list_room[i].pos_x == x && list_room[i].pos_y == y)
                return list_room[i];
        }
        return null;
    }
    public void CheckRoomState()
    {
        ///TODO
        GameManager.instance.player.currentState.Value =  Character.STATE.Idling;
    }
}
