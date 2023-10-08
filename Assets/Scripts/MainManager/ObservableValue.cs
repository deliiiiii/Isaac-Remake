using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
            T oldValue = this.value;
            if (this.value.Equals(value))
                return;
            //if (typeof(T) == typeof(int) && (int.Parse(value.ToString()) < 0))
            //    return;

            //if (valueType == 7 && (int.Parse(value.ToString()) < 0))
            //{
            //    T t = (T)(object)Convert.ToInt32(0);
            //    this.value = t;
            //}

            this.value = value;
            OnValueChangeEvent?.Invoke(oldValue, value, this.valueType);
        }
    }
    public void OnValueChange(T oldValue, T newValue, int valueType)
    {
        
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
            //case 0://根据UI/战斗状态进行更新
            //{
            //    GameManager.instance.RefreshState();
            //    break;
            //}
            case 1://更新血量UI
            {
                //Debug.Log("HP :" + oldValue + " -> " + newValue);
                UIManager.instance.RefreshHPUI();
                break;
            }
            case 2://更新Item UI
            {
                UIManager.instance.RefreshItemUI();
                break;
            }
            case 3://更新生成器UI
            {
                UIManager.instance.RefreshGeneratorUI();
                break;
            }
            case 4://更新摄像头位置
            {
                //Debug.Log(CameraController.instance == null);
                CameraController.instance.CallRefreshPosition();
                break;
            }
            //case 5://更新房门状态
            //{
            //    RoomManager.instance.RefreshDoorState();
            //    break;
            //}
            case 6://更新角色状态
            {
                if(typeof(T) == typeof(Character.STATE))
                {
                    if (newValue.Equals(Character.STATE.Idling))
                    {
                        GameManager.instance.player.GetComponent<BoxCollider2D>().enabled = true;
                        GameManager.instance.player.GetComponent<CircleCollider2D>().enabled = true;
                    }
                    if(newValue.Equals(Character.STATE.ChangingRoom))
                    {
                        GameManager.instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        GameManager.instance.player.GetComponent<Character>().SetAnim_Move(null);
                        GameManager.instance.player.GetComponent<BoxCollider2D>().enabled = false;
                        GameManager.instance.player.GetComponent<CircleCollider2D>().enabled = false;
                    }
                }
                break;
            }
            case 7://更新Block状态
            {
                RoomManager.instance.RefreshBlockState();
                break;
            }
            default:
                break;
        }
    }
}
