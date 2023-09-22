using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DPScript.Loading;

namespace DPScript.Editor
{
    public class BattleDebugMenu : EditorWindow
    {
        bool isStarted = false;
        bool worldSettings = false, worldSong = false, playerSpawning = false;
        List<bool> activePlayers = new List<bool>(), activeTransform = new List<bool>(), activeSet = new List<bool>(), activeSound = new List<bool>();
        List<string> soundToPlay = new List<string>(), stateToEnter = new List<string>(), effectToCall = new List<string>();
        bool commonPlayerEffect = false, inputCheckBool = false;
        List<Vector3> effectSpawnOffsets = new List<Vector3>();
        List<int> effectSpawnDeathTime = new List<int>();
        string songToPlay = "";

        string objectToSpawn = "";
        int objectSpawnColor = 0, inputTypeCheck = 0;

        [MenuItem("Window/DPScript/Battle Debug Menu")]
        public static void create()
        {
            GetWindow<BattleDebugMenu>("Battle Debug Menu");
        }

        void Start()
        {
            for (int i = 0; i < Battle_Manager.Instance.players.Count; i++)
            {
                soundToPlay.Add("");
                stateToEnter.Add("");
                effectToCall.Add("");
                effectSpawnOffsets.Add(Vector3.zero);
                effectSpawnDeathTime.Add(0);
                activePlayers.Add(false);
                activeTransform.Add(false);
                activeSet.Add(false);
                activeSound.Add(false);
            }
        }

        void OnGUI()
        {
            if (Application.isPlaying)
            {
                if (Battle_Manager.Instance == null)
                {
                    GUILayout.Label("No Battle Manager Avilable"); //i spelt that so fucking wrong but i dont wanna google it :skull:
                    return;
                }
                else if(!isStarted)
                {
                    Start();
                }
            }
            else
            {
                if (isStarted)
                {
                    isStarted = false;
                    soundToPlay.Clear();
                    stateToEnter.Clear();
                    effectToCall.Clear();
                    effectSpawnOffsets.Clear();
                    effectSpawnDeathTime.Clear();
                    activePlayers.Clear();
                    activeTransform.Clear();
                    activeSet.Clear();
                    activeSound.Clear();
                }
                GUILayout.Label("No Battle Manager Avilable"); //i spelt that so fucking wrong but i dont wanna google it :skull:
                return;
            }

            worldSettings = EditorGUILayout.Foldout(worldSettings, "World Settings");
            if (worldSettings)
            {
                Battle_Manager.Instance.showHitboxes = EditorGUILayout.Toggle("Show Hitboxes", Battle_Manager.Instance.showHitboxes);

                worldSong = EditorGUILayout.Foldout(worldSong, "Song");
                if (worldSong)
                {
                    if (GUILayout.Button("Play"))
                        Battle_Music.Instance.playMusic(true);
                    if (GUILayout.Button("Pause"))
                        Battle_Music.Instance.playMusic(false);

                    songToPlay = EditorGUILayout.TextField("Song Path", songToPlay);
                    if (GUILayout.Button("Switch"))
                        Battle_Music.Instance.setSong(Resources.Load<AudioClip>(songToPlay));
                    if (GUILayout.Button("Reset Song"))
                        Battle_Music.Instance.resetSong();
                }

                playerSpawning = EditorGUILayout.Foldout(playerSpawning, "Player Spawning");
                if (playerSpawning)
                {
                    objectToSpawn = EditorGUILayout.TextField("Load path", objectToSpawn);
                    objectSpawnColor = EditorGUILayout.IntField("Color", objectSpawnColor);
                    if (GUILayout.Button("Spawn"))
                    {
                        //since this isnt a monobehaviour well just make the battle manager start the coroutine
                        Battle_Manager.Instance.StartCoroutine(Objects_Load.mainLoad(Resources.Load<DPS_ObjectLoad>(objectToSpawn), objectSpawnColor));
                        soundToPlay.Add("");
                        stateToEnter.Add("");
                        effectToCall.Add("");
                        effectSpawnOffsets.Add(Vector3.zero);
                        effectSpawnDeathTime.Add(0);
                    }
                }
            }

            for (int i = 0; i < Battle_Manager.Instance.players.Count; i++)
            {
                GameWorldObject player = Battle_Manager.Instance.players[i];
                activePlayers[i] = EditorGUILayout.Foldout(activePlayers[i], "Player " + (i + i) + " Options");
                if (activePlayers[i])
                {
                    activeTransform[i] = EditorGUILayout.Foldout(activeTransform[i], "Transform");
                    if (activeTransform[i])
                    {
                        player.loc = EditorGUILayout.Vector3IntField("Position", player.loc);
                        player.impulse = EditorGUILayout.Vector3IntField("Impulse", player.impulse);
                        player.impulseAdd = EditorGUILayout.Vector3IntField("Impulse Modifier", player.impulseAdd);
                        player.rotation = EditorGUILayout.Vector3Field("Rotation", player.rotation);
                        player.scale = EditorGUILayout.Vector3Field("Scale", player.scale);

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("<"))
                            player.dir = ObjectDir.dir_Left;
                        if (GUILayout.Button(">"))
                            player.dir = ObjectDir.dir_Right;
                        GUILayout.Label("Set Direction");
                        EditorGUILayout.EndHorizontal();

                        player.faceCamera = EditorGUILayout.Toggle("Face Camera", player.faceCamera);
                    }

                    activeSet[i] = EditorGUILayout.Foldout(activeSet[i], "Player Settings");
                    if (activeSet[i])
                    {
                        player.ignoreInputs = EditorGUILayout.Toggle("Ignore Inputs", player.ignoreInputs);
                        player.ignoreFreezes = EditorGUILayout.Toggle("Ignore Freezes", player.ignoreFreezes);

                        stateToEnter[i] = EditorGUILayout.TextField("State to enter", stateToEnter[i]);
                        if (GUILayout.Button("Enter"))
                            DPS_ObjectCommand.enterState(stateToEnter[i], player);
                        if (GUILayout.Button("Exit State"))
                            DPS_ObjectCommand.exitState(player);

                        commonPlayerEffect = EditorGUILayout.Toggle("Load common player effect", commonPlayerEffect);
                        effectToCall[i] = EditorGUILayout.TextField("Effect to play", effectToCall[i]);
                        effectSpawnOffsets[i] = EditorGUILayout.Vector3Field("Effect Offset", effectSpawnOffsets[i]);
                        effectSpawnDeathTime[i] = EditorGUILayout.IntField("Effect Life Time", effectSpawnDeathTime[i]);
                        if (GUILayout.Button("Spawn Effect"))
                        {
                            if (commonPlayerEffect)
                                Battle_Manager.Instance.commonPlayer.spawnEffect(effectToCall[i], effectSpawnOffsets[i], 
                                    (uint)effectSpawnDeathTime[i], player);
                            else
                                player.effectManager.spawnEffect(effectToCall[i], effectSpawnOffsets[i], (uint)effectSpawnDeathTime[i]);
                        }

                        inputTypeCheck = EditorGUILayout.IntField("Check Input", inputTypeCheck);
                        if (GUILayout.Button("Check"))
                            inputCheckBool = player.input_CanInput((short)inputTypeCheck, "", 1, 0);
                        GUILayout.Label(inputCheckBool.ToString());
                    }

                    activeSound[i] = EditorGUILayout.Foldout(activeSound[i], "Sounds");
                    if (activeSound[i])
                    {
                        soundToPlay[i] = EditorGUILayout.TextField("Sound to play", soundToPlay[i]);
                        if (GUILayout.Button("Play Sound"))
                            player.audioManager.playSoundEffect(soundToPlay[i]);
                        if (GUILayout.Button("Play Voice Line"))
                            player.audioManager.playVoiceLine(soundToPlay[i]);
                    }
                }
            }
            GUILayout.Label((int)(Time.deltaTime * 1000) + "ms");
        }
    }
}