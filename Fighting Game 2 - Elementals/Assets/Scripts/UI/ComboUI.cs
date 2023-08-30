using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI comboCounterText;
    [SerializeField] TextMeshProUGUI hitTypeText;

    public void SetCombo(object sender, int combo)
    {
        if(combo <= 1)
        {
            comboCounterText.text = "";
            return;
        }

        comboCounterText.text = combo.ToString() + "x";
    }

    public void SetHitType(object sender, string text)
    {
        hitTypeText.text = text;
    }
}