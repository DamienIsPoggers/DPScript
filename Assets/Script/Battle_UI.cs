using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DPScript;

public class Battle_UI : MonoBehaviour
{
    GameWorldObject p1, p2;
    [SerializeField]
    Image p1Portrait, p2Portrait, p1SuperBarNum, p2SuperBarNum, timer;
    [SerializeField]
    Slider p1Slider, p2Slider;
    [SerializeField]
    List<Sprite> timerFont, superBarFont;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
