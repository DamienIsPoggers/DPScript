using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;
using ImGuiNET;
using System.Drawing;
using Steamworks;
using UnityEngine.UI;

public class HitboxOverlay : MonoBehaviour
{

    void OnEnable()
    {
        ImGuiUn.Layout += Draw;
    }

    void OnDisable()
    {
        ImGuiUn.Layout -= Draw;
    }

    void Draw()
    {
        if (!Battle_Manager.Instance.showHitboxes)
            return;

        ImGui.GetWindowDrawList().AddQuadFilled(Vector2.right, Vector2.up, Vector2.left, Vector2.down, 0xFFFF0000);

        /*
        foreach (Object_Collision box in Battle_Manager.Instance.collisions)
        {
            Vector2 pointA = new Vector2(((box.baseBox.x / 100) + box.parent.transform.position.x) * box.parent.transform.localScale.x * box.parent.dir, 
                ((box.baseBox.y / 100) + box.parent.transform.position.y) * box.parent.transform.localScale.y);
            Vector2 pointB = new Vector2((((box.baseBox.x + box.baseBox.width) / 100) + box.parent.transform.position.x) * box.parent.transform.localScale.x * box.parent.dir,
                ((box.baseBox.y / 100) + box.parent.transform.position.y) * box.parent.transform.localScale.y);
            Vector2 pointC = new Vector2((((box.baseBox.x + box.baseBox.width) / 100) + box.parent.transform.position.x) * box.parent.transform.localScale.x * box.parent.dir,
                (((box.baseBox.y = box.baseBox.height) / 100) + box.parent.transform.position.y) * box.parent.transform.localScale.y);
            Vector2 pointD = new Vector2(((box.baseBox.x / 100) + box.parent.transform.position.x) * box.parent.transform.localScale.x * box.parent.dir,
                (((box.baseBox.y + box.baseBox.height) / 100) + box.parent.transform.position.y) * box.parent.transform.localScale.y);

            pointA = Camera.main.WorldToScreenPoint(pointA);
            pointB = Camera.main.WorldToScreenPoint(pointB);
            pointC = Camera.main.WorldToScreenPoint(pointC);
            pointD = Camera.main.WorldToScreenPoint(pointD);

            Debug.Log(pointA.ToString());

            uint outlineColor, boxColor;
            switch(box.type)
            {
                default:
                    outlineColor = 0xFFFFFFFF;
                    boxColor = 0x78FFFFFF;
                    break;
                case 0:
                    outlineColor = 0xFFFFDC00;
                    boxColor = 0x78FFDC00;
                    return;
                case 1:
                    outlineColor = 0xFF0033CC;
                    boxColor = 0x780033CC;
                    return;
                case 2:
                    outlineColor = 0xFFFF0000;
                    boxColor = 0x78FF0000;
                    return;
            }

            ImGui.GetWindowDrawList().AddQuad(pointA, pointB, pointC, pointD, outlineColor, 1);
            ImGui.GetWindowDrawList().AddQuadFilled(pointA, pointB, pointC, pointD, boxColor);
        }
        */
        
    }
}
