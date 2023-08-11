using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DPScript;
using JetBrains.Annotations;

public class Battle_UI : MonoBehaviour
{
    public static Battle_UI Instance;

    GameWorldObject p1, p2;
    [Header("Images")]
    [SerializeField]
    Image p1Portrait;
    [SerializeField]
    Image p2Portrait, p1SuperBarNum, p2SuperBarNum, timer, p1HeathBar, p2HealthBar;
    [Header("Sliders")]
    [SerializeField]
    Slider p1HealthBarSlider;
    [SerializeField]
    Slider p2HealthBarSlider;
    [Header("Scripts")]
    [SerializeField]
    List<UI_ComboCounter> comboCounters = new List<UI_ComboCounter>();
    [Header("Transforms")]
    [SerializeField]
    RectTransform p1Pointer;
    [SerializeField]
    RectTransform p2Pointer;
    public List<Sprite> timerFont, superBarFont, comboCounterFont, invalidComboCounterFont, damageNumberFont;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
}
