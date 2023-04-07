using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using DPScript;
using System.Text;

public class DebugMenu : MonoBehaviour
{
    DebugShit inputStuff;
    bool show = false;

    List<byte[]> soundToPlay = new List<byte[]>();
    List<byte[]> stateToEnter = new List<byte[]>();
    byte[] songToPlay = new byte[50];

    void Start()
    {
        for (int i = 0; i < Battle_Manager.Instance.players.Count; i++)
        {
            soundToPlay.Add(new byte[32]);
            stateToEnter.Add(new byte[32]);
        }
    }

    void Awake()
    {
        inputStuff = new DebugShit();
    }

    void OnEnable()
    {
        ImGuiUn.Layout += Draw;
        inputStuff.Debug.Enable();
    }

    void OnDisable()
    {
        ImGuiUn.Layout -= Draw;
        inputStuff.Debug.Disable();
    }

    void Update()
    {
        if (inputStuff.Debug.DebugMenu.WasPressedThisFrame())
            show = !show;
    }

    void Draw()
    {
        if (!show)
            return;

        if (ImGui.Begin("Debug Menu"))
        {
            if (ImGui.TreeNode("World Settings"))
            {
                ImGui.Checkbox("Show Hitboxes", ref Battle_Manager.Instance.showHitboxes);
                
                if(ImGui.TreeNode("Song"))
                {
                    if (ImGui.Button("Play"))
                        Battle_Music.Instance.playMusic(true);
                    if (ImGui.Button("Pause"))
                        Battle_Music.Instance.playMusic(false);

                    ImGui.SetNextItemWidth(150f);
                    ImGui.InputText("Song Path", songToPlay, 50);
                    if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                        ImGui.SetTooltip("Will switch the current song to this one");
                    if (ImGui.Button("Switch"))
                        Battle_Music.Instance.setSong(Resources.Load<AudioClip>(Encoding.ASCII
                            .GetString(songToPlay).Replace("\0", string.Empty)));
                    if (ImGui.Button("Reset Song"))
                        Battle_Music.Instance.resetSong();

                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            for (int i = 0; i < Battle_Manager.Instance.players.Count; i++)
                if(ImGui.TreeNode("Player " + (i + 1) + " Options"))
                {
                    if (ImGui.TreeNode("Transform"))
                    {
                        ImGui.DragInt3("Position", ref Battle_Manager.Instance.players[i].locX, 100f);
                        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                            ImGui.SetTooltip("The objects position.");
                        ImGui.DragInt3("Impulse", ref Battle_Manager.Instance.players[i].xImpulse, 100f);
                        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                            ImGui.SetTooltip("The objects impulse. Updates the position every frame.");
                        ImGui.DragInt3("Impulse Modifier", ref Battle_Manager.Instance.players[i].xImpulseAdd, 10f);
                        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                            ImGui.SetTooltip("The objects impulse modifier. Updates the impulse every frame.");
                        ImGui.DragFloat3("Rotation", ref Battle_Manager.Instance.players[i].rotation);
                        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                            ImGui.SetTooltip("The objects rotation.");
                        ImGui.DragFloat3("Scale", ref Battle_Manager.Instance.players[i].scale, 0.1f);
                        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                            ImGui.SetTooltip("The objects scale.");

                        if (ImGui.ArrowButton("l", ImGuiDir.Left))
                            Battle_Manager.Instance.players[i].dir = -1;
                        ImGui.SameLine();
                        if (ImGui.ArrowButton("r", ImGuiDir.Right))
                            Battle_Manager.Instance.players[i].dir = 1;
                        ImGui.SameLine();
                        ImGui.Text("Set Direction");
                        

                        ImGui.TreePop();
                    }

                    if(ImGui.TreeNode("Player Settings"))
                    {
                        ImGui.SetNextItemWidth(150f);
                        ImGui.InputText("State to enter", stateToEnter[i], 32);
                        if (ImGui.Button("Enter"))
                            Battle_Manager.Instance.players[i].GetComponent<DPS_ObjectCommand>()
                                .enterState(Encoding.ASCII.GetString(stateToEnter[i]).Replace("\0", string.Empty));

                        ImGui.TreePop();
                    }

                    if(ImGui.TreeNode("Sounds"))
                    {
                        ImGui.SetNextItemWidth(150f);
                        ImGui.InputText("Sound to play", soundToPlay[i], 32);
                        if (ImGui.Button("Play Sound"))
                            Battle_Manager.Instance.players[i].GetComponent<DPS_AudioManager>()
                                .playSoundEffect(Encoding.ASCII.GetString(soundToPlay[i]).Replace("\0", string.Empty));
                        if (ImGui.Button("Play Voice Line"))
                            Battle_Manager.Instance.players[i].GetComponent<DPS_AudioManager>()
                                .playVoiceLine(Encoding.ASCII.GetString(soundToPlay[i]).Replace("\0", string.Empty));

                        ImGui.TreePop();
                    }

                    ImGui.TreePop();
                }
        }
    }
}
