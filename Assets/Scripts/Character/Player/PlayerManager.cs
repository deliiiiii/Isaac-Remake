using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<GameObject> prefab_player = new();
    private void Awake()
    {
        instance = this;
    }

    public void ReadPlayer()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            instance.prefab_player.Add(gameObject.transform.GetChild(i).gameObject);
            instance.prefab_player[i].GetComponent<Character>().index = i;
        }
    }
    public void SetPlayer(int index_player,bool IsRestart)
    {
        //Debug.Log("SetPlayer index = " + index_player);
        if(GameManager.instance.player)
        {
            if(!IsRestart)
            {
                Transform oldTranform = GameManager.instance.player.transform;
                Character oldCharacter = GameManager.instance.player.GetComponent<Character>();
                Tear oldTear = GameManager.instance.player.GetComponent<Character>().tear;

                GameManager.instance.player.SetActive(false);
                GameManager.instance.player = Instantiate(prefab_player[index_player], gameObject.transform);
                GameManager.instance.player.SetActive(true);

                GameManager.instance.player.transform.SetPositionAndRotation(oldTranform.position, oldTranform.rotation);
                GameManager.instance.player.transform.localScale = oldTranform.localScale;

                GameManager.instance.player.GetComponent<Character>().hasEnabled = true;

                GameManager.instance.player.GetComponent<Character>().curHP = new(-1, 1);
                GameManager.instance.player.GetComponent<Character>().maxHP = new(-1, 1);
                GameManager.instance.player.GetComponent<Character>().tempHP = new(-1, 1);
                GameManager.instance.player.GetComponent<Character>().blackHP = new(-1, 1);
                GameManager.instance.player.GetComponent<Character>().curHP.Value = oldCharacter.curHP.Value;
                GameManager.instance.player.GetComponent<Character>().maxHP.Value = oldCharacter.maxHP.Value;
                GameManager.instance.player.GetComponent<Character>().tempHP.Value = oldCharacter.tempHP.Value;
                GameManager.instance.player.GetComponent<Character>().blackHP.Value = oldCharacter.blackHP.Value;

                GameManager.instance.player.GetComponent<Character>().hurtCD = oldCharacter.hurtCD;
                GameManager.instance.player.GetComponent<Character>().hurtTimer = oldCharacter.hurtTimer;
                GameManager.instance.player.GetComponent<Character>().isHurt = oldCharacter.isHurt;

                GameManager.instance.player.GetComponent<Character>().c_height = oldCharacter.c_height;
                GameManager.instance.player.GetComponent<Character>().moveSpeed = oldCharacter.moveSpeed;
                GameManager.instance.player.GetComponent<Character>().frictionSpeed = oldCharacter.frictionSpeed;

                GameManager.instance.player.GetComponent<Character>().tear = oldTear;
                GameManager.instance.player.GetComponent<Character>().tearShootCD = oldCharacter.tearShootCD;
                GameManager.instance.player.GetComponent<Character>().tearShootTimer = oldCharacter.tearShootTimer;
                GameManager.instance.player.GetComponent<Character>().tearSpeed = oldCharacter.tearSpeed;
                GameManager.instance.player.GetComponent<Character>().tearSpeedDivisionWhileMoving = oldCharacter.tearSpeedDivisionWhileMoving;
                GameManager.instance.player.GetComponent<Character>().tearRange = oldCharacter.tearRange;
                GameManager.instance.player.GetComponent<Character>().tearDamage = oldCharacter.tearDamage;
                GameManager.instance.player.GetComponent<Character>().tearSize = oldCharacter.tearSize;
            }

            else
            {
                GameManager.instance.player.SetActive(false);
                GameManager.instance.player = Instantiate(prefab_player[index_player], gameObject.transform);
                GameManager.instance.player.SetActive(true);
            }
            //Destroy(transform.GetChild(prefab_player.Count).gameObject);
        }
        else
        {
            GameManager.instance.player = Instantiate(prefab_player[index_player], gameObject.transform);
            GameManager.instance.player.SetActive(true);
        }


    }
}
