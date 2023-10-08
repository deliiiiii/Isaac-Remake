using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isExploring = false;

    public GameObject player;
    public enum GAMESTATE
    {
        Menu_Main,
        Menu_Idle,
        Menu_Paused,
        Menu_GameOver,
        Room_Idle,
        //Room_Battle,
    }
    public ObservableValue<GAMESTATE> gameState = new (GAMESTATE.Menu_Main, 0);
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EnemyManager.instance.ReadEnemy();
        PlayerManager.instance.ReadPlayer();
        BlockManager.instance.ReadBlock();
        ItemManager.instance.ReadItem();
        UIManager.instance.Initialize();

        //StartNewGame();


        //RoomManager.instance.currentRoom.Value.GenerateBlock(0, 5);
        //RoomManager.instance.currentRoom.Value.GenerateBlock(1, 10);
        //RoomManager.instance.currentRoom.Value.GenerateBlock(2, 8);
        //RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 0,true);
        //RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 1,true);
        //RoomManager.instance.currentRoom.Value.GenerateItem(RoomManager.instance.currentRoom.Value.transform, 2,true);
    }
    //public void SetGameState(GAMESTATE state)
    //{
    //    gameState.Value = state;
    //}
    public void StartNewGame()
    {
        Time.timeScale = 1f;
        isExploring = true;
        UIManager.instance.text_Continue.GetComponent<Image>().sprite = UIManager.instance.sprite_Continue_T;
        gameState.Value = GAMESTATE.Room_Idle;
        PlayerManager.instance.SetPlayer(0, true);
        RoomManager.instance.GenerateFloor();
    }
    public bool ResumeGame()
    {
        if (!isExploring)
            return false;
        Time.timeScale = 1f;
        gameState.Value = GAMESTATE.Room_Idle;
        return true;
    }
    public void EndLife()
    {
        Time.timeScale = 0;
        gameState.Value = GAMESTATE.Menu_GameOver;
        isExploring = false;
        UIManager.instance.text_Continue.GetComponent<Image>().sprite = UIManager.instance.sprite_Continue_F;
        UIManager.instance.MySetActive(UIManager.instance.panel_Restart);
        Destroy(player);

        
        
        
        
    }
}
