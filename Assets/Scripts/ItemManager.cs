using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private ObservableValue<int> count_penny = new ObservableValue<int>(0, 2);
    private ObservableValue<int> count_bomb = new ObservableValue<int>(1, 2);
    private ObservableValue<int> count_key = new ObservableValue<int>(0, 2);

    public Text text_penny;
    public Text text_bomb;
    public Text text_key;

    private void Awake()
    {
        instance = this;
        RefreshItemUI();
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

    public void RefreshItemUI()
    {
        instance.text_penny.text = instance.count_penny.Value.ToString();
        instance.text_bomb.text = instance.count_bomb.Value.ToString();
        instance.text_key.text = instance.count_key.Value.ToString();
    }
}
