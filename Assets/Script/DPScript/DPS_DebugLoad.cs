using DPScript.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DPScript
{
    public class DPS_DebugLoad : MonoBehaviour
    {
        [SerializeField]
        string char1LoadPath = "", char2LoadPath = "";
        [SerializeField]
        int char1LoadColor = 0, char2LoadColor = 0, playerSpawnPos = 0;

        void Awake()
        {
            StartCoroutine(Objects_Load.mainLoad(Resources.Load<DPS_ObjectLoad>(char1LoadPath), char1LoadColor, new InputDevice[] { Keyboard.current }, true, 0,
                -playerSpawnPos));
            if (Gamepad.all.Count <= 0)
                StartCoroutine(Objects_Load.mainLoad(Resources.Load<DPS_ObjectLoad>(char2LoadPath), char2LoadColor, true, 1, playerSpawnPos));
            else
                StartCoroutine(Objects_Load.mainLoad(Resources.Load<DPS_ObjectLoad>(char2LoadPath), char2LoadColor, new InputDevice[] { Gamepad.all[0] }, true, 1, 
                    playerSpawnPos));
        } 
    }
}