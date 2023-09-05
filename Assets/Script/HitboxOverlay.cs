using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPScript;
using System.Drawing;
using Steamworks;
using UnityEngine.UI;

public class HitboxOverlay : MonoBehaviour
{
    /*
    void Draw()
    {
        if (!Battle_Manager.Instance.showHitboxes)
            return;

        //ImGui.GetForegroundDrawList().AddQuadFilled(new Vector2(300, 300), new Vector2(400, 300), new Vector2(400, 400), new Vector2(300, 400), 0xFFFF0000);

        
        foreach (Object_Collision box in Battle_Manager.Instance.collisions)
        {
            

            uint outlineColor, boxColor;
            switch(box.box.type)
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
        }
    }
    */
}
