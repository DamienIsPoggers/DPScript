using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DPScript;
using DPScript.Loading;
using System;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect Instance;
    Csel_Icon[,] iconMap;
    public Vector2Int[] playerPos = { new Vector2Int(0, 0), new Vector2Int(0, 0) };
    public StageInCsel cselStage = StageInCsel.PlayerNumSelect;
    public PlayerStageInCsel[] playerStage = { PlayerStageInCsel.Waiting, PlayerStageInCsel.Waiting };
    [SerializeField]
    int highestX = 0, highestY = 0;
    [SerializeField]
    DPS_CharSelectLoad[] loadedChars = { null, null };
    [SerializeField]
    float[] charUnloadTime = { 0, 0 }; 
    [SerializeField]
    List<InputDevice> devices = new List<InputDevice>();
    [SerializeField]
    sbyte p1Controller = -1, p2Controller = -1;
    public byte singlePlayerCharNum = 0;
    public bool[] singlePlayerSelected = { false, false };
    public int controllersAdded = 0;

    void Start()
    {
        Csel_Icon[] icons = FindObjectsOfType<Csel_Icon>();
        foreach(Csel_Icon icon in icons)
        {
            if (icon.Position.x > highestX)
                highestX = icon.Position.x;
            if (icon.Position.y > highestY)
                highestY = icon.Position.y;
        }
        iconMap = new Csel_Icon[highestX, highestY];
        foreach (Csel_Icon icon in icons)
            iconMap[icon.Position.y, icon.Position.x] = icon;
    }

    void Update()
    {
        
    }

    public void inputUpdate(Vector2Int move, bool accept, bool escape, int controllerNum)
    {
        if (cselStage != StageInCsel.PlayerNumSelect && (controllerNum != p1Controller || controllerNum != p2Controller))
            return;

        int player = 0;
        if (controllerNum == p2Controller)
            player = 1;

        switch(cselStage)
        {
            case StageInCsel.CharacterSelect:
                switch(playerStage[player])
                {
                    case PlayerStageInCsel.SelectingCharacter:
                        playerPos[player] += move;

                        if (playerPos[player].x >= highestX)
                            playerPos[player].x = 0;
                        if (playerPos[player].x < 0)
                            playerPos[player].x = highestX - 1;

                        if (playerPos[player].y >= highestY)
                            playerPos[player].y = 0;
                        if (playerPos[player].y < 0)
                            playerPos[player].y = highestY - 1;

                        if (accept)
                            updatePlayerState(PlayerStageInCsel.SelectingColor, player);
                        if (escape)
                            updateCselState(StageInCsel.PlayerNumSelect);
                        break;

                }
                break;
        }
    }

    public void updatePlayerState(PlayerStageInCsel stateTo, int playerNum)
    {

    }

    public void updateCselState(StageInCsel stateTo)
    {

    }

    public enum StageInCsel
    {
        PlayerNumSelect,
        CharacterSelect,
        P1Select,
        P2Select,
        SongSelect,
        StageSelect,
    }

    public enum PlayerStageInCsel
    {
        Waiting,
        SelectingCharacter,
        SelectingColor,
        Done,
    }
}
