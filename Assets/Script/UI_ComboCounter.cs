using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_ComboCounter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("0 indexed. So if player 1 counter set as 0.")]
    int playerNumber = 0;
    [SerializeField]
    Image digit1, digit2, digit3, oldDigit1, oldDigit2, oldDigit3;
    [SerializeField]
    Animator numbersAnimator, comboEndAnimator;
    [SerializeField]
    [Tooltip("Limits to use next animation for combo finishers. If lv 2 combo animation is at 5 hits then set index 0 to 5")]
    List<int> comboLvLimits = new List<int>();
    List<Sprite> font, invalidFont, damageFont;
    GameWorldObject player;
    bool comboIsValid = true;

    int previousCount = 0;

    static readonly int[] maxCombo = { 9, 9, 9 };

    void Start()
    {
        font = Battle_UI.Instance.comboCounterFont;
        invalidFont = Battle_UI.Instance.invalidComboCounterFont;
        damageFont = Battle_UI.Instance.damageNumberFont;
    }

    void Update()
    {
        if(player == null)
        {
            if (Battle_Manager.Instance.players.Count >= playerNumber)
                player = Battle_Manager.Instance.players[playerNumber];
            else
                return;
        }

        if(comboIsValid != player.comboIsValid)
        {
            comboIsValid = player.comboIsValid;
            updateComboType();
        }

        if (player.comboCounter <= 1 || player.comboCounter == previousCount)
            goto TimerUpdate;

        if (player.comboCounter > 999)
            return;

        int[] old = getDigits(previousCount);
        int[] nums = getDigits(player.comboCounter);

        updateComboNumber(old, nums);

        TimerUpdate:

        if(!player.isInCombo)
        {
            numbersAnimator.Play("ComboEnd");
            string anim = "ComboEndLv";
            int lv = 1;
            for(int i = 0; i < comboLvLimits.Count; i++)
            {
                if (previousCount < comboLvLimits[i])
                    lv++;
                else
                    break;
            }
            anim += lv.ToString();
            comboEndAnimator.Play(anim);
        }
    }

    int[] getDigits(int value)
    {
        List<int> numbers = new List<int>();

        for (; value > 0; value /= 10)
            numbers.Add(value % 10);

        return numbers.ToArray();
    }

    void updateComboType()
    {
        int[] nums;
        if (previousCount >= 999)
            nums = maxCombo;
        else
            nums = getDigits(previousCount);

        updateComboNumber(nums, nums);
    }

    void updateComboNumber(int[] old, int[] nums)
    {
        if (comboIsValid)
        {
            digit1.sprite = font[nums[0]];
            oldDigit1.sprite = font[old[0]];
            if(nums.Length > 1)
                digit2.sprite = font[nums[1]];
            if(old.Length > 1)
                oldDigit2.sprite = font[old[1]];
            if (nums.Length > 2)
                digit3.sprite = font[nums[2]];
            if (old.Length > 2)
                oldDigit3.sprite = font[old[2]];
        }
        else
        {
            digit1.sprite = invalidFont[nums[0]];
            oldDigit1.sprite = invalidFont[old[0]];
            if (nums.Length > 1)
                digit2.sprite = invalidFont[nums[1]];
            if (old.Length > 1)
                oldDigit2.sprite = invalidFont[old[1]];
            if (nums.Length > 2)
                digit3.sprite = invalidFont[nums[2]];
            if (old.Length > 2)
                oldDigit3.sprite = invalidFont[old[2]];
        }

        numbersAnimator.Play("ComboCounterUpdate");
    }
}
