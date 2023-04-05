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

    void Start()
    {
        soundToPlay.Add(new byte[32]);
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
            ImGui.Checkbox("Show Hitboxes", ref Battle_Manager.Instance.showHitboxes);

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
                        ImGui.DragFloat3("Scale", ref Battle_Manager.Instance.players[i].scale);
                        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                            ImGui.SetTooltip("The objects scale.");

                        ImGui.TreePop();
                    }

                    if(ImGui.TreeNode("Sounds"))
                    {
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
