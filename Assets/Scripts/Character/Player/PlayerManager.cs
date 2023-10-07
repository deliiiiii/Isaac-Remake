using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<GameObject> prefab_player = new();
    public GameObject curPlayer = null;
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
    public void SetPlayer(int index_player)
    {
        Debug.Log("SetPlayer index = " + index_player);
        if(GameManager.instance.player)
        {
            //Transform oldTranform = GameManager.instance.player.transform;
            //Character oldCharacter = GameManager.instance.player;


            GameManager.instance.player.gameObject.SetActive(false);
            GameManager.instance.player = prefab_player[index_player].GetComponent<Character>();
            GameManager.instance.player.gameObject.SetActive(true);

            //GameManager.instance.player.transform.SetPositionAndRotation(oldTranform.position, oldTranform.rotation);
            //GameManager.instance.player.transform.localScale = oldTranform.localScale;

            //GameManager.instance.player.curHP.Value = oldCharacter.curHP.Value;
            //GameManager.instance.player.maxHP.Value = oldCharacter.maxHP.Value;
            //GameManager.instance.player.tempHP.Value = oldCharacter.tempHP.Value;
            //GameManager.instance.player.blackHP.Value = oldCharacter.blackHP.Value;

            //GameManager.instance.player.hurtCD = oldCharacter.hurtCD;
            //GameManager.instance.player.hurtTimer = oldCharacter.hurtTimer;
            //GameManager.instance.player.isHurt = oldCharacter.isHurt;

            //GameManager.instance.player.c_height = oldCharacter.c_height;
            //GameManager.instance.player.moveSpeed = oldCharacter.moveSpeed;
            //GameManager.instance.player.frictionSpeed = oldCharacter.frictionSpeed;


            //GameManager.instance.player.tear = oldCharacter.tear;
            //GameManager.instance.player.tearShootCD = oldCharacter.tearShootCD;
            //GameManager.instance.player.tearShootTimer = oldCharacter.tearShootTimer;
            //GameManager.instance.player.tearSpeed = oldCharacter.tearSpeed;
            //GameManager.instance.player.tearRange = oldCharacter.tearRange;
            //GameManager.instance.player.tearDamage = oldCharacter.tearDamage;
            //GameManager.instance.player.tearSize = oldCharacter.tearSize;

            //GameManager.instance.player = oldCharacter;
        }
        else
        {
            GameManager.instance.player = prefab_player[index_player].GetComponent<Character>();
            GameManager.instance.player.gameObject.SetActive(true);
        }


    }
}
