using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DPScript;
using DPScript.Loading;
using System;
using UnityEngine.InputSystem;
using System.Linq;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect Instance;
    [Header("Data")]
    [SerializeField]
    Csel_Icon[,] iconMap;
    public Vector2Int[] playerPos = { new Vector2Int(0, 0), new Vector2Int(0, 0) };
    public StageInCsel cselStage = StageInCsel.PlayerNumSelect;
    public PlayerStageInCsel[] playerStage = { PlayerStageInCsel.Waiting, PlayerStageInCsel.Waiting };
    public int[] playerColor = { 0, 0 };
    [SerializeField]
    int highestX = 0, highestY = 0;
    [SerializeField]
    bool[] loadingChar = { false, false };
    [SerializeField]
    DPS_CharSelectLoad[] loadedChars = { null, null };
    [SerializeField]
    string[] loadIds = { "", "" };
    [SerializeField]
    GameObject[] charObjects = { null, null };
    [SerializeField]
    float[] charUnloadTime = { 0, 0 }, playerNoControlTime = { 0, 0 };
    [SerializeField]
    float globalNoControlTime = 0;
    [SerializeField]
    List<InputDevice> devices = new List<InputDevice>();
    [SerializeField]
    int[] playerControllers = { -1, -1 };
    public byte singlePlayerCharNum = 0;
    public bool[] singlePlayerSelected = { false, false };
    public int controllersAdded = 0;
    [Header("Transforms")]
    [SerializeField]
    RectTransform[] playerPortraitParents = { null, null };
    [SerializeField]
    Transform[] playerObjectParents = { null, null };
    [Header("Anims")]
    [SerializeField]
    float[] spriteAlphaTime = { 1, 1 };

    void Awake()
    {
        Instance = this;
        Csel_Icon[] icons = FindObjectsOfType<Csel_Icon>();
        foreach(Csel_Icon icon in icons)
        {
            if (icon.Position.x > highestX)
                highestX = icon.Position.x;
            if (icon.Position.y > highestY)
                highestY = icon.Position.y;
        }
        iconMap = new Csel_Icon[highestX + 1, highestY + 1];
        foreach (Csel_Icon icon in icons)
            iconMap[icon.Position.x, icon.Position.y] = icon;
    }

    void Update()
    {
        if (playerNoControlTime[0] > 0)
            playerNoControlTime[0] -= Time.deltaTime;
        if (playerNoControlTime[1] > 0)
            playerNoControlTime[1] -= Time.deltaTime;

        if (globalNoControlTime > 0)
            globalNoControlTime -= Time.deltaTime;

        for(int i = 0; i < 2; i++)
        {
            if (!loadingChar[i])
            {
                if (loadedChars[i] != null)
                {
                    if (charObjects[i] == null)
                        charObjects[i] = Instantiate(loadedChars[i].prefab, playerObjectParents[i]);
                }
                if (loadIds[i] != iconMap[playerPos[i].x, playerPos[i].y].charDef.LoadString)
                    charUnloadTime[i] -= Time.deltaTime;
                else
                    charUnloadTime[i] = 1.5f;

                if (charUnloadTime[i] <= 0)
                {
                    Destroy(charObjects[i]);
                    charObjects[i] = null;
                    Resources.UnloadAsset(loadedChars[i]);
                    loadedChars[i] = null;
                    StartCoroutine(loadChar(iconMap[playerPos[i].x, playerPos[i].y].charDef.LoadString, i));
                }
            }

            if (loadedChars[i] != null)
            {

                if (loadedChars[i].isSprite)
                {
                    charObjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Lerp(0, 1, spriteAlphaTime[i]));
                    spriteAlphaTime[i] += Time.deltaTime;
                }
            }
        }
    }

    public void inputUpdate(Vector2Int move, bool accept, bool escape, int controllerNum)
    {
        if (cselStage != StageInCsel.PlayerNumSelect && !playerControllers.Contains(controllerNum))
            return;

        Debug.Log("now here");

        int player = 0;
        if (controllerNum == playerControllers[1])
            player = 1;

        if (playerNoControlTime[player] > 0 || globalNoControlTime > 0)
            return;

        switch(cselStage)
        {
            case StageInCsel.CharacterSelect:
                goto Character;
        }

        return;

    Character:
        switch (playerStage[player])
        {
            case PlayerStageInCsel.SelectingCharacter:
                playerPos[player].x += move.x;
                playerPos[player].y += move.y;

                if (playerPos[player].x > highestX)
                    playerPos[player].x = 0;
                if (playerPos[player].x < 0)
                    playerPos[player].x = highestX;

                if (playerPos[player].y > highestY)
                    playerPos[player].y = 0;
                if (playerPos[player].y < 0)
                    playerPos[player].y = highestY;

                if (accept)
                    updatePlayerState(PlayerStageInCsel.SelectingColor, player);
                if (escape)
                    updateCselState(StageInCsel.PlayerNumSelect);
                break;
            case PlayerStageInCsel.SelectingColor:
                playerColor[player] += move.y;

                if (playerColor[player] >= loadedChars[player].mats.mats.Count)
                    playerColor[player] = 0;
                if (playerColor[player] < 0)
                    playerColor[player] = loadedChars[player].mats.mats.Count - 1;

                if (accept)
                    updatePlayerState(PlayerStageInCsel.Done, player);
                if (escape)
                    updatePlayerState(PlayerStageInCsel.SelectingCharacter, player);
                break;
            case PlayerStageInCsel.Done:
                if (escape)
                    updatePlayerState(PlayerStageInCsel.SelectingColor, player);
                break;
        }
        playerNoControlTime[player] = 0.5f;
    }

    public void updatePlayerState(PlayerStageInCsel stateTo, int playerNum)
    {
        playerStage[playerNum] = stateTo;
        playerNoControlTime[playerNum] = 2.5f;
        if (stateTo == PlayerStageInCsel.SelectingCharacter)
            playerColor[playerNum] = 0;
    }

    public void updateCselState(StageInCsel stateTo)
    {

    }

    IEnumerator loadChar(string id, int playerNum)
    {
        loadingChar[playerNum] = true;
        ResourceRequest chr = Resources.LoadAsync<DPS_CharSelectLoad>("char/" + id + "/" + id + "/" + id + "_csel");
        if (!chr.isDone)
            yield return null;
        charUnloadTime[playerNum] = 1.5f;
        loadedChars[playerNum] = (DPS_CharSelectLoad)chr.asset;
        loadIds[playerNum] = id;
        spriteAlphaTime[playerNum] = 0;
        loadingChar[playerNum] = false;
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
