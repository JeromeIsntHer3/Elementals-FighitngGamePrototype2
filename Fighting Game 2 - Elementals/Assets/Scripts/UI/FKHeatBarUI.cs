using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FKHeatBarUI : MonoBehaviour
{
    [SerializeField] Image barFillImage;
    [SerializeField] Vector2 positionalValue;
    

    public void SetHeatMeterValue(float value)
    {
        barFillImage.fillAmount = value;
    }

    public void SetupHeatBar(bool left)
    {
        GetComponent<RectTransform>().anchoredPosition = left ? new Vector2(positionalValue.x, positionalValue.y) :
            new Vector2(-positionalValue.x, positionalValue.y);
        barFillImage.fillOrigin = left ? (int)Image.OriginHorizontal.Left : (int)Image.OriginHorizontal.Right;
    }
}