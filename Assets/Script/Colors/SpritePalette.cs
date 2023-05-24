using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePalette : MonoBehaviour
{
    public Texture2D colors;
    public bool run = false;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    // Update is called once per frame
    void Update()
    {
        if (!run)
            return;
        Texture2D sprite = spriteRenderer.sprite.texture;
        for (int i = 0; i < sprite.height; i++)
            for(int i2 = 0; i2 < sprite.width; i2++)
            {
                Color color = sprite.GetPixel(i2, i);
                if (color.a == 0)
                    continue;
                sprite.SetPixel(i2, i, colors.GetPixel(Convert.ToInt32(color.r*255), Convert.ToInt32(color.b*255)));
            }
    }
}
