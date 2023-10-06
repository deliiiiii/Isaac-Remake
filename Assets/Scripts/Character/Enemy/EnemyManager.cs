using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<Character> prefab_enemy = new ();//automatically read items(prefabs) when AWAKE
    private void Awake()
    {
        instance = this;
    }

    public void ReadEnemy()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            instance.prefab_enemy.Add(gameObject.transform.GetChild(i).GetComponent<Character>());
            instance.prefab_enemy[i].index = i;
        }
    }


}
