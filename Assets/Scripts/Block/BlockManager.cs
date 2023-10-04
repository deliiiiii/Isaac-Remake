using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;

    public List<Block> prefab_block = new List<Block>();//automatically read blocks(prefabs) when AWAKE
    private void Awake()
    {
        instance = this;
    }
    public void ReadBlock()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            instance.prefab_block.Add(gameObject.transform.GetChild(i).GetComponent<Block>());
            instance.prefab_block[i].index = i;
        }
    }
}
