using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class DebugMenu : MonoBehaviour
{
    DebugShit inputStuff;
    bool show = false;

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
        }
    }
}
