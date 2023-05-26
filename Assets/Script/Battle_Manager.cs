using DPScript;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    public static Battle_Manager Instance;

    public bool superFreeze = false;
    public List<DPS_Stage> stages = new List<DPS_Stage>();
    public bool spriteCharacterOnScreen = false;
    public bool showHitboxes = false;
    public int stateWidth = 0;

    public List<GameWorldObject> players = new List<GameWorldObject>();
    public DPS_CommonPlayer commonPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
