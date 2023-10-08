using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Player Info")]
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

    [Header("Custom Generator")]
    public GameObject panel_CustomGenerator;
    public Button button_MinusType;
    public Button button_PlusType;
    public Button button_MinusStuff;
    public Button button_PlusStuff;
    public Button button_MinusCount;
    public Button button_PlusCount;
    public Text text_ObjectType;
    public Text text_StuffName;
    public Text text_StuffCount;
    public Button button_Generate;
    private List<string> string_stuffType= new (){ "Enemy", "Item","Block" };
    private List<List<string>> string_stuffName = new List<List<string>>();
    private ObservableValue<int> index_objectType = new(0,3);
    private ObservableValue<int> index_stuffName = new(0,3);
    private ObservableValue<int> index_stuffCount = new(0, 3);
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            panel_CustomGenerator.SetActive(!panel_CustomGenerator.activeSelf);
        if(panel_CustomGenerator.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
                GenerateStuff();
            if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                Minus_index_stuffCount();
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                Plus_index_stuffCount();
            if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
                Minus_index_stuffName();
            if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
                Plus_index_stuffName();
            if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
                Minus_index_objectType();
            if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
                Plus_index_objectType();

        }
        


    }
    public void Initialize()
    {
        button_MinusType.onClick.AddListener(Minus_index_objectType);
        button_PlusType.onClick.AddListener(Plus_index_objectType);
        button_MinusStuff.onClick.AddListener(Minus_index_stuffName);
        button_PlusStuff.onClick.AddListener(Plus_index_stuffName);
        button_MinusCount.onClick.AddListener(Minus_index_stuffCount);
        button_PlusCount.onClick.AddListener(Plus_index_stuffCount);
        button_Generate.onClick.AddListener(GenerateStuff);

        AddTextToList();
        ReadStuffName();
    }
    public void ReadStuffName()
    {
        string_stuffName.Add(new());
        for (int i = 0; i < EnemyManager.instance.prefab_enemy.Count; i++)
        {
            string_stuffName[0].Add(EnemyManager.instance.prefab_enemy[i].name);
        }
        string_stuffName.Add(new());
        for (int i = 0; i < ItemManager.instance.prefab_item.Count; i++)
        {
            string_stuffName[1].Add(ItemManager.instance.prefab_item[i].name);
        }
        string_stuffName.Add(new());
        for (int i = 0; i < BlockManager.instance.prefab_block.Count; i++)
        {
            string_stuffName[2].Add(BlockManager.instance.prefab_block[i].name);
        }
        RefreshGeneratorUI();
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
        int filledRedHP = GameManager.instance.player.GetComponent<Character>().curHP.Value;
        int maxRedHP = GameManager.instance.player.GetComponent<Character>().maxHP.Value;
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
    public void RefreshGeneratorUI()
    {
        if (index_objectType.Value == 0)
            button_MinusType.interactable = false;
        else
            button_MinusType.interactable = true;
        if(index_objectType.Value == string_stuffType.Count - 1)
            button_PlusType.interactable = false;
        else
            button_PlusType.interactable = true;

        if (index_stuffName.Value == 0)
            button_MinusStuff.interactable = false;
        else
            button_MinusStuff.interactable = true;
        if (index_stuffName.Value == string_stuffName[index_objectType.Value].Count - 1)
            button_PlusStuff.interactable = false;
        else
            button_PlusStuff.interactable = true;

        if (index_stuffCount.Value == 0)
        {
            button_MinusCount.interactable = false;
            button_Generate.interactable = false;
        }
        else
        {
            button_MinusCount.interactable = true;
            button_Generate.interactable = true;
        }
        if (index_stuffCount.Value == 9)
            button_PlusCount.interactable = false;
        else
            button_PlusCount.interactable = true;
        text_ObjectType.text = string_stuffType[index_objectType.Value];
        text_StuffName.text = string_stuffName[index_objectType.Value][index_stuffName.Value];
        text_StuffCount.text = index_stuffCount.Value.ToString();
    }
    private void GenerateStuff()
    {
        Debug.Log("GeneratorUI : " + index_objectType.Value + " " + index_stuffName.Value + " " + index_stuffCount.Value);
        switch(index_objectType.Value)
        {
            case 0:
            {
                for(int i=0;i<index_stuffCount.Value;i++)
                {
                    RoomManager.instance.currentRoom.Value.GenerateEnemy(RoomManager.instance.currentRoom.Value.transform, index_stuffName.Value);
                }
                break;
            }
            case 1:
            {
                for (int i = 0; i < index_stuffCount.Value; i++)
                {
                    RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, index_stuffName.Value,true);
                }
                break;
            }
            case 2:
            {
                RoomManager.instance.currentRoom.Value.GenerateBlock(index_stuffName.Value, index_stuffCount.Value);
                break;
            }   
        }
    }
    public void Plus_index_objectType()
    {
        index_stuffName.Value = 0;
        if(index_objectType.Value < string_stuffType.Count - 1)
            index_objectType.Value++;
        
    }
    public void Minus_index_objectType()
    {
        index_stuffName.Value = 0;
        if (index_objectType.Value > 0)
            index_objectType.Value--;
       
    }
    public void Plus_index_stuffName()
    {
        if(index_stuffName.Value < string_stuffName[index_objectType.Value].Count - 1)
            index_stuffName.Value++;
    }
    public void Minus_index_stuffName()
    {
        if (index_stuffName.Value > 0)
            index_stuffName.Value--;
    }
    public void Plus_index_stuffCount()
    {
        if(index_stuffCount.Value < 9)
        index_stuffCount.Value++;
    }
    public void Minus_index_stuffCount()
    {
        if (index_stuffCount.Value > 0)
        index_stuffCount.Value--;
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
