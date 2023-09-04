using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterUI : MonoBehaviour
{
    [SerializeField] Image meterFillImg;
    [SerializeField] TextMeshProUGUI meterCountText;

    public void OnMeterUsed(object sender, BaseCharacterAttacks.OnMeterUsedArgs args)
    {
        meterFillImg.fillAmount = args.amount / GameManager.MaxMeterValue;
        meterCountText.text = args.count.ToString();
    }
}