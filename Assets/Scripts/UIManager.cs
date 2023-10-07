using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject redHP_Full;
    public GameObject redHP_Half;
    public GameObject redHP_Empty;
    public GameObject blueHP_Full;
    public GameObject blueHP_Empty;
    public GameObject panel_HP;

    public Text text_penny;
    public Text text_bomb;
    public Text text_key;
    public List<Text> list_ItemText;


    private void Awake()
    {
        instance = this;
    }
    public void AddTextToList()
    {
        list_ItemText = new List<Text>()
        {
            text_penny,
            text_bomb,
            text_key,
        };
        RefreshItemUI();
    }
    public void RefreshItemUI()
    {
        for (int i = 0; i < list_ItemText.Count; i++)
        {
            instance.list_ItemText[i].text = ItemManager.instance.prefab_item[i].count.Value.ToString();
            if (ItemManager.instance.prefab_item[i].count.Value < 10 && ItemManager.instance.prefab_item[i].count.Value >= 0)
                instance.list_ItemText[i].text = "0" + ItemManager.instance.prefab_item[i].count.Value.ToString();
        }
    }

    public void RefreshHPUI()
    {
        ClearChild(panel_HP);
        int generatedHP = 0;
        int filledRedHP = GameManager.instance.player.curHP.Value;
        int maxRedHP = GameManager.instance.player.maxHP.Value;
        for (int i=0;i+1< filledRedHP; i+=2)
        {
            Instantiate(redHP_Full, panel_HP.transform);
            generatedHP += 2;
        }
        if(filledRedHP % 2 == 1)
        {
            Instantiate(redHP_Half, panel_HP.transform);
            generatedHP += 2;
        }
        for(int i=0;i< (maxRedHP - generatedHP)/2;i++)
        {
            Instantiate(redHP_Empty, panel_HP.transform);
        }
            
    }
    private void ClearChild(GameObject parent)
    {
        Transform transform;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            transform = parent.transform.GetChild(i);
            Destroy(transform.gameObject);
        }
        //for (int i = 0;i<parent.transform.childCount;i++)
        //    Destroy(parent.transform.GetChild(i).gameObject);
    }
}
