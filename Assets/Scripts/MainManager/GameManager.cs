using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    //private enum GameState
    //{
    //    Menu_Idle,
    //    Menu_Paused,
    //    Menu_GameOver,
    //    Room_Idle,
    //    Room_Battle,
    //}
    //private ObservableValue<GameState>gameState = new ObservableValue<GameState>(GameState.Menu_Idle,0);
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //instance.gameState.Value = GameState.Menu_Idle; 
        EnemyManager.instance.ReadEnemy();
        PlayerManager.instance.ReadPlayer();
        PlayerManager.instance.SetPlayer(0);

        RoomManager.instance.GenerateFloor();
        BlockManager.instance.ReadBlock();
        ItemManager.instance.ReadItem();
        UIManager.instance.Initialize();


        

        //RoomManager.instance.currentRoom.Value.GenerateBlock(0, 5);
        //RoomManager.instance.currentRoom.Value.GenerateBlock(1, 10);
        //RoomManager.instance.currentRoom.Value.GenerateBlock(2, 8);
        //RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 0,true);
        //RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 1,true);
        //RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 2,true);
    }
    //public void RefreshState()
    //{
    //    Debug.Log("RefreshState");
    //}
}
