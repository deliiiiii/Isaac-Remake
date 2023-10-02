using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableValue<T>
{
    private T value;
    private readonly int valueType;
    /*public */
    delegate void OnValueChangeDelegate(T oldValue, T newValue, int valueType);
    /*public */
    event OnValueChangeDelegate OnValueChangeEvent;
    public ObservableValue(T value, int valueType)
    {
        this.value = value;
        this.valueType = valueType;
        this.OnValueChangeEvent += OnValueChange;

    }
    public T Value
    {
        get => value;
        set
        {
            if (this.value.Equals(value))
                return;
            if (typeof(T) == typeof(int) && (int.Parse(value.ToString()) < 0))
                return;
            OnValueChangeEvent?.Invoke(this.value, value, this.valueType);
            this.value = value;
        }
    }
    public void OnValueChange(T oldValue, T newValue, int valueType)
    {
        //Debug.Log("oldValue = " + oldValue + " newValue = " + newValue);
        //if (valueType == 0 && typeof(T) == typeof(int) && int.Parse(newValue.ToString()) >= 2)
        //{
        //    //Debug.Log("int达到数值2 ！！");
        //}
        //if (valueType == 1 && typeof(T) == typeof(float) && float.Parse(newValue.ToString()) >= 0.2f)
        //{
        //    //Debug.Log("float达到数值0.2f ！！");
        //}
        switch(valueType)
        {
            case 2://更新Item UI
                ItemManager.instance.RefreshItemUI();
                break;
            default:
                break;
        }

    }
}
