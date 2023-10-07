using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<Item> prefab_item = new List<Item>();//automatically read items(prefabs) when AWAKE

    //public Text text_penny;
    //public Text text_bomb;
    //public Text text_key;
    //public List<Text> list_ItemText;
    private void Awake()
    {
        instance = this;
    }
    public void ReadItem()
    {
        //list_ItemText = new List<Text>()
        //{
        //    text_penny,
        //    text_bomb,
        //    text_key,
        //};
        for (int i = 0;i<gameObject.transform.childCount;i++)
        {
            instance.prefab_item.Add(gameObject.transform.GetChild(i).GetComponent<Item>());
            instance.prefab_item[i].index = i;
        }
        prefab_item[0].count.Value = 0;
        prefab_item[1].count.Value = 1;
        prefab_item[2].count.Value = 2;
        //RefreshItemUI();
    }

    //public void RefreshItemUI()
    //{
    //    for(int i = 0; i<list_ItemText.Count;i++)
    //    {
    //        instance.list_ItemText[i].text = instance.prefab_item[i].count.Value.ToString();
    //        if (instance.prefab_item[i].count.Value < 10 && instance.prefab_item[i].count.Value >= 0)
    //            instance.list_ItemText[i].text = "0" + instance.prefab_item[i].count.Value.ToString();
    //    }
    //}
}
