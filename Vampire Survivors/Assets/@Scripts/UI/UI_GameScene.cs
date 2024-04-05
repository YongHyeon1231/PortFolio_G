using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Base
{
    [SerializeField]
    TextMeshProUGUI _killCountText;

    [SerializeField]
    Slider _gemslider;

    public void SetGemGountRatio(float ratio)
    {
        _gemslider.value = ratio;
    }

    public void SetSkillCount(int killCount)
    {
        _killCountText.text = $"{killCount}";
    }
}
