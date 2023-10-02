using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private ObservableValue<int> count_penny;
    private ObservableValue<int> count_bomb;
    private ObservableValue<int> count_key;

    public Text text_penny;
    public Text text_bomb;
    public Text text_key;

    private void Awake()
    {
        count_penny = new ObservableValue<int>(-1, 2);
        count_bomb = new ObservableValue<int>(-1, 2);
        count_key = new ObservableValue<int>(-1, 2);
        instance = this; 
        Initialize();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            instance.count_penny.Value++;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            instance.count_bomb.Value++;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            instance.count_key.Value++;
    }
    private void Initialize()
    {
        //instance.count_penny.Value = 0;
        //instance.count_bomb.Value = 1;
        instance.count_key.Value = 0;
    }

    public void RefreshItemUI()
    {
        instance.text_penny.text = instance.count_penny.Value.ToString();
        instance.text_bomb.text = instance.count_bomb.Value.ToString();
        instance.text_key.text = instance.count_key.Value.ToString();
    }
}
