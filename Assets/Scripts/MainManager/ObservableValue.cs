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
        //    //Debug.Log("int�ﵽ��ֵ2 ����");
        //}
        //if (valueType == 1 && typeof(T) == typeof(float) && float.Parse(newValue.ToString()) >= 0.2f)
        //{
        //    //Debug.Log("float�ﵽ��ֵ0.2f ����");
        //}
        
        switch(valueType)
        {
            //case 0://����UI/ս��״̬���и���
            //{
            //    GameManager.instance.RefreshState();
            //    break;
            //}
            case 1://����Ѫ��UI
            {
                //Debug.Log("HP :" + oldValue + " -> " + newValue);
                UIManager.instance.RefreshHPUI();
                break;
            }
            case 2://����Item UI
            {
                UIManager.instance.RefreshItemUI();
                break;
            }
            case 3://����������UI
            {
                UIManager.instance.RefreshGeneratorUI();
                break;
            }
            case 4://��������ͷλ��
            {
                //Debug.Log(CameraController.instance == null);
                CameraController.instance.CallRefreshPosition();
                break;
            }
            //case 5://���·���״̬
            //{
            //    RoomManager.instance.RefreshDoorState();
            //    break;
            //}
            case 6://���½�ɫ״̬
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
            case 7://����Block״̬
            {
                RoomManager.instance.RefreshBlockState();
                break;
            }
            case 8://����MenuUI
            {
                UIManager.instance.RefreshMenuUI();
                break;
            }
            default:
                break;
        }
    }
}