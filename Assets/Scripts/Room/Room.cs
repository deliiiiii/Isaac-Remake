using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Room : MonoBehaviour
{
    public int pos_x;//小地图坐标(1,1)
    public int pos_y;
    public Vector3 pos_world;//中心点的世界坐标
    public List<Door> doors = new();
    public List<Block> blocks = new();
    public List<Item> items = new();
    public List<ObservableValue<int>> state_door = new() { new(0,5), new(0, 5), new(0, 5), new(0, 5), new(0, 5) };
    private void Awake()
    {
        pos_world = new Vector3(transform.position.x-0.5f,transform.position.y-0.5f,transform.position.z);
        for(int i=0;i<=4;i++)
        {
            doors.Add(transform.GetChild(i).gameObject.GetComponent<Door>());
        }
    }
    public void GenerateBlock(int block_index , int count)
    {
        int max_trial =66;
        int block_pos_x, block_pos_y;
        while(count > 0 && max_trial >= 0)
        {
            max_trial--;
            block_pos_x = UnityEngine.Random.Range(-6,7);
            block_pos_y = UnityEngine.Random.Range(-3,4);
            if (CheckExistBlock(block_pos_x, block_pos_y))
            {
                continue;
            }
            blocks.Add(Instantiate(BlockManager.instance.prefab_block[block_index].gameObject, 
                        new Vector3(RoomManager.instance.currentRoom.Value.pos_world.x + block_pos_x,
                                    RoomManager.instance.currentRoom.Value.pos_world.y + block_pos_y,
                                    0f),
                        Quaternion.identity,
                        RoomManager.instance.currentRoom.Value.transform).GetComponent<Block>());
            blocks[^1].gameObject.SetActive(true);
            blocks[^1].pos_x = block_pos_x;
            blocks[^1].pos_y = block_pos_y;
            count--;
            
        }
    }
    public void GenerateItem(Transform transform,int item_index,bool CanCollect)
    {
        items.Add(Instantiate(ItemManager.instance.prefab_item[item_index].gameObject,
                  transform.position,
                  Quaternion.identity,
                  transform).GetComponent<Item>());
        items[^1].gameObject.SetActive(true);
        if (CanCollect)
            return;
        if (items[^1].index == 1)
        {
            items[^1].transform.position = new Vector3(items[^1].transform.position.x, items[^1].transform.position.y - 0.65f, 0f);
            items[^1].transform.SetParent(this.transform,true);
            items[^1].GetComponent<Rigidbody2D>().velocity = new Vector2(transform.gameObject.GetComponent<Rigidbody2D>().velocity.x / 5f, transform.gameObject.GetComponent<Rigidbody2D>().velocity.y / 5f);
            items[^1].GetComponent<Bomb>().SetAnim_before_Explode();
            items[^1].canCollect = false;
        }
            
    }
    private bool CheckExistBlock(int pos_x, int pos_y)
    {
        for(int i=0;i<blocks.Count;i++)
        {
            if (blocks[i].pos_x == pos_x && blocks[i].pos_y == pos_y)
                return true;
        }
        return false;
    }
}
