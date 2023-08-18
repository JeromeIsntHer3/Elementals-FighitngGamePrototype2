using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterUI : MonoBehaviour
{
    [SerializeField] Image meterFillImg;
    [SerializeField] TextMeshProUGUI meterCountText;
    [SerializeField] BaseCharacterAttacks owner;

    public void Setup(BaseCharacterAttacks owner)
    {
        this.owner = owner;
        owner.OnMeterUsed += OnMeterUsed;
    }

    void OnDisable()
    {
        owner.OnMeterUsed -= OnMeterUsed;
    }

    void OnMeterUsed(object sender, BaseCharacterAttacks.OnMeterUsedArgs args)
    {
        meterFillImg.fillAmount = args.MeterValue;
        meterCountText.text = args.MeterCount.ToString();
    }
}