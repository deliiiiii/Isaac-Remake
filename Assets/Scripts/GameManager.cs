using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private enum GameState
    {
        Menu_Idle,
        Menu_Paused,
        Menu_GameOver,
        Room_Idle,
        Room_Battle,
    }
    private ObservableValue<GameState>gameState = new ObservableValue<GameState>(GameState.Menu_Idle,0);
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        instance.gameState.Value = GameState.Menu_Idle;
    }
    public void RefreshState()
    {
        Debug.Log("RefreshState");
    }
}
