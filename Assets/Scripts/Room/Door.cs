using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    private CircleCollider2D circleCollider2d;
    public Sprite sprite_openNormal;
    public Sprite sprite_closedWithoutLock;
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
    public STATE state;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        circleCollider2d = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        
    }
    public void SetState(STATE newState,bool isM)
    {
        state = newState;
        switch (state)
        {
            case STATE.Disabled:
                {
                    SetActiveDoor(false, false);
                    break;
                }

            case STATE.openNormal:
                {
                    SetActiveDoor(true, false);
                    GetComponent<SpriteRenderer>().sprite = sprite_openNormal;
                    break;
                }
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
            case STATE.openAward:
                break;
            case STATE.openBoss:
                break;
            case STATE.openEvil:
                break;
            case STATE.closed_normal:
                {
                    SetActiveDoor(true, true);
                    GetComponent<SpriteRenderer>().sprite = sprite_closedWithoutLock;
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
            RoomManager.instance.RefreshDoorState(newState);
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
}
