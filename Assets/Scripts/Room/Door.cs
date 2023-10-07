using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    private CircleCollider2D circleCollider2d;
    public Sprite sprite_openNormal;
    public Sprite sprite_closed_normal;
    public Sprite sprite_closedWithOneLock;
    //public Sprite sprite_closedWithTwoLock;
    public enum STATE
    {
        Disabled,
        openNormal,
        openAward,
        openBoss,
        openEvil,

        closed_normal,
        closed_1_Lock,
        closed_2_Lock,

        openAfterBattling,
        closedWhenBattling,
    }
    public STATE base_state;
    private STATE temp_state;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        circleCollider2d = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        
    }
    public void SetBaseState(STATE newBaseState,bool isM)
    {
        base_state = newBaseState;
        switch (newBaseState)
        {
            case STATE.Disabled:
                {
                    SetActiveDoor(false, false);
                    GetComponent<SpriteRenderer>().sprite = null;
                    break;
                }
            case STATE.openNormal:
                {
                    SetActiveDoor(true, false);
                    GetComponent<SpriteRenderer>().sprite = sprite_openNormal;
                    break;
                }
            //case STATE.openAfterBattling:
            //    {
            //        SetActiveDoor(true, false);
            //        break;
            //    }
            //case STATE.closedWhenBattling:
            //    {
            //        SetActiveDoor(true, true);
            //        break;
            //    }
            case STATE.openAward:
                break;
            case STATE.openBoss:
                break;
            case STATE.openEvil:
                break;
            case STATE.closed_normal:
                {
                    SetActiveDoor(true, true);
                    GetComponent<SpriteRenderer>().sprite = sprite_closed_normal;
                    break;
                }
            case STATE.closed_1_Lock:
                {
                    SetActiveDoor(true, false);
                    GetComponent<SpriteRenderer>().sprite = sprite_closedWithOneLock;
                    break;
                }
            case STATE.closed_2_Lock:
                break;
            
            default:
                break;
        }
        if(!isM)
            RoomManager.instance.RefreshDoorState(newBaseState);
    }
    public void SetNewState(STATE newTempState,bool isM)
    {
        //Debug.Log("TempState = " + newTempState);
        temp_state = newTempState;
        if(base_state == STATE.Disabled)
        {
            return;
        }
        switch(newTempState)
        {
            case STATE.openAfterBattling:
                {
                    SetActiveDoor(true, false);
                    break;
                }
            case STATE.closedWhenBattling:
                {
                    SetActiveDoor(true, true);
                    break;
                }
            default: break;
        }
        SetDoorSprite(base_state, temp_state);
        if (!isM)
            RoomManager.instance.RefreshDoorState(newTempState);
    }
    private void SetActiveDoor(bool active,bool closed)
    {
        if(!active)
        {
            gameObject.tag = "Block";
            spriteRenderer.enabled = false;
            boxCollider2d.enabled = true;
            circleCollider2d.enabled = false;
        }
        else
        {
            gameObject.tag = "Door";
            spriteRenderer.enabled = true;
            boxCollider2d.enabled = closed;
            circleCollider2d.enabled = true;
        }
    }

    private void SetDoorSprite(STATE baseState,STATE tempState)
    {
        if(tempState == STATE.openAfterBattling)
        {
            //if (baseState == STATE.closed_1_Lock)
            //    return;
            //if (baseState == STATE.closed_2_Lock)
            //    return;
            if (baseState == STATE.openNormal)
                GetComponent<SpriteRenderer>().sprite = sprite_openNormal;
        }
        if(tempState == STATE.closedWhenBattling)
        {
            if (baseState == STATE.openNormal)
                GetComponent<SpriteRenderer>().sprite = sprite_closed_normal;
        }
    }
}
