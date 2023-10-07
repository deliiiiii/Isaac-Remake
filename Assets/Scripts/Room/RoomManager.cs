using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public List<Room> list_room = new();
    public ObservableValue<Room> currentRoom = new(new(),4);
    private int min_x;
    private int min_y;
    private int max_x;
    private int max_y;
    private int max_distance = -1;
    //private int width = 3;
    //private int column = 3;


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
    //public void AddRoomToList()
    //{
    //    for (int i = 1; i <= width; i++)
    //        for (int j = 1; j <= column; j++)
    //        {
    //            GenerateRoom(i, j, Room.ROOMTYPE.initial);
    //        }
    //}

    public void GenerateFloor()
    {
        ClearFloor();
        GenerateRoom(Room.ROOMTYPE.initial);
        int ran = /*UnityEngine.Random.Range(1, 2)*/1;
        switch(ran)
        {
            case 1:
                {
                    for(int i=0;i<3;i++)
                    {
                        GenerateRoom(Room.ROOMTYPE.battle);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        GenerateRoom(Room.ROOMTYPE.item);
                    }
                    if(!GenerateRoom(Room.ROOMTYPE.boss))
                    {
                        Debug.Log("Generate Boss Room Failed!");
                        GenerateFloor();
                        return;
                    }
                break;
                }
               
            default:
                break;
        }
        
        currentRoom.Value = list_room[0]; 
        GameManager.instance.player.transform.position = currentRoom.Value.pos_world;
        CameraController.instance.CallRefreshPosition();
        RefreshDoorState();
    }
    private void ClearFloor()
    {
        for (int i = 1; i < grid.transform.childCount; i++)
        {
            Destroy(grid.transform.GetChild(i).gameObject);
        }
        list_room.Clear();
        min_x = min_y = max_x = max_y = 0;
        max_distance = -1;
    }
    private bool GenerateRoom(Room.ROOMTYPE roomType)
    {
        if(roomType == Room.ROOMTYPE.initial)
        {
            list_room.Add(Instantiate(room_default, new Vector3(0 * 35, 0 * 24, 0), Quaternion.identity, grid.transform).GetComponent<Room>());
            list_room[^1].pos_x = 0;
            list_room[^1].pos_y = 0;
        }
        else
        {
            int ran_x = -1;
            int ran_y = -1;
            int max_trial = 66;
            while(max_trial > 0)
            {
                max_trial--;
                ran_x = UnityEngine.Random.Range(min_x - 1, max_x + 2);
                ran_y = UnityEngine.Random.Range(min_y - 1, max_y + 2);
                //Debug.Log("RandomRoom Trial :" + max_trial + " (" + ran_x + "," + ran_y + ")");
                if (RoomDistance(ran_x,ran_y,0,0) < max_distance * 0.65f)
                    continue;
                if(roomType == Room.ROOMTYPE.boss && RoomDistance(ran_x, ran_y, 0, 0) < max_distance - 2)
                    continue;
                if (roomType == Room.ROOMTYPE.boss && RoomDistance(ran_x, ran_y, 0, 0) <= 2)
                    continue;
                if (FindRoom(ran_x, ran_y))
                    continue;
                
                int direction;
                int count_exit = 0;
                for (direction = 1; direction <= 4; direction++)
                {
                    Room hasFoundRoom = FindRoom(ran_x + dir_or_index_door[0, direction], ran_y + dir_or_index_door[1, direction]);
                    if (hasFoundRoom != null)
                    {
                        count_exit++;
                        if (hasFoundRoom.type == Room.ROOMTYPE.item)
                        {
                            count_exit = 0;
                            break;
                        }
                    }
                       
                }
                if (count_exit == 0)
                    continue;
                if(count_exit >= 2 && ( roomType == Room.ROOMTYPE.item || roomType == Room.ROOMTYPE.boss))
                    continue;
                else
                    break;
            }
            if (max_trial == 0)
                return false;
            Debug.Log("New Room : " + " (" + ran_x + "," + ran_y + ")" + roomType);
            list_room.Add(Instantiate(room_default, new Vector3(ran_x * 30,ran_y * 18, 0), Quaternion.identity, grid.transform).GetComponent<Room>());
            list_room[^1].pos_x = ran_x;
            list_room[^1].pos_y = ran_y;
            min_x = Mathf.Min(min_x, ran_x);
            min_y = Mathf.Min(min_y, ran_y);
            max_x = Mathf.Max(max_x, ran_x);
            max_y = Mathf.Max(max_y, ran_y);
            max_distance = Mathf.Max(max_distance, RoomDistance(ran_x, ran_y, 0, 0));
        }
        
        list_room[^1].type = roomType;
        list_room[^1].gameObject.SetActive(true);

        return true;
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
                    if (hasFoundRoom.type == Room.ROOMTYPE.item)
                        list_room[i].doors[direction].SetBaseState(Door.STATE.closed_1_Lock, true);
                    else if (hasFoundRoom.type == Room.ROOMTYPE.battle || hasFoundRoom.type == Room.ROOMTYPE.initial)
                        list_room[i].doors[direction].SetBaseState(Door.STATE.openNormal, true);
                    else if (hasFoundRoom.type == Room.ROOMTYPE.boss)
                        list_room[i].doors[direction].SetBaseState(Door.STATE.openBoss, true);
                    
                    if (list_room[i].type == Room.ROOMTYPE.item)
                        list_room[i].doors[direction].SetBaseState(Door.STATE.closed_1_Lock, true);
                    else if (list_room[i].type == Room.ROOMTYPE.boss)
                        list_room[i].doors[direction].SetBaseState(Door.STATE.openBoss, true);
                }
                else
                    list_room[i].doors[direction].SetBaseState(Door.STATE.Disabled, true);
            }
        }
    }
    public void RefreshDoorState(Door.STATE changedState)
    {
        for (int i = 0; i < list_room.Count; i++)
        {
            for (int direction = 1; direction <= 4; direction++)
            {
                if (list_room[i].doors[direction].base_state == changedState)
                {
                    Room nextRoom = FindRoom(list_room[i].pos_x + dir_or_index_door[0, direction], list_room[i].pos_y + dir_or_index_door[1, direction]);
                    //Debug.Log("nextRoom : " + nextRoom.pos_x + " " + nextRoom.pos_y);
                    if (nextRoom == null)
                    {
                        Debug.LogError("Not found nextRoom!!");
                        continue;
                    }
                    //Debug.Log("New Door Direction : " + dir_or_index_door[2, direction]);
                    nextRoom.doors[dir_or_index_door[2, direction]].SetBaseState(changedState,true);
                }

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
        if(targetRoom == null)
        {
            Debug.LogError("There's no nextRoom!");
            return Vector3.zero;
        }
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
        if(!currentRoom.Value.hasExplored)
        {
            if(currentRoom.Value.type == Room.ROOMTYPE.battle)
            {
                currentRoom.Value.hasExplored = true;
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform, UnityEngine.Random.Range(0, EnemyManager.instance.prefab_enemy.Count));
            }
            else if(currentRoom.Value.type == Room.ROOMTYPE.item)
            {
                currentRoom.Value.hasExplored = true;
                currentRoom.Value.GenerateItem(currentRoom.Value.transform,UnityEngine.Random.Range(0,ItemManager.instance.prefab_item.Count),false);
            }
            else if (currentRoom.Value.type == Room.ROOMTYPE.boss)
            {
                currentRoom.Value.hasExplored = true;
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,0);
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,0);
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,1);
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,1);
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,1);
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,2);
                currentRoom.Value.GenerateEnemy(currentRoom.Value.transform,3);

            }
        }
        GameManager.instance.player.GetComponent<Character>().currentState.Value =  Character.STATE.Idling;
    }
    public int RoomDistance(int x1,int y1,int x2,int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }
}
