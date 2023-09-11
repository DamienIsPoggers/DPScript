using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState = GameState.Battle;
    public bool GameIsPaused = false;
    public bool p1IsKeyboardAndController = true;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    
    void Update()
    {

    }
}

[Serializable]
public enum GameState
{
    Title,
    Video,
    MainMenu,
    Csel,
    Battle,
    Init,
}
