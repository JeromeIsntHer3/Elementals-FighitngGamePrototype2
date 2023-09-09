using UnityEngine;
using UnityEngine.UI;

public class FKHeatBarUI : MonoBehaviour
{
    [SerializeField] Image barFillImage;

    RectTransform rt;
    FKAttacks manager;

    public void SetupHeatBar(FKAttacks attacks, int index)
    {
        rt = GetComponent<RectTransform>();
        manager = attacks;
        manager.OnHeatValueChanged += UpdateHeatValue;

        switch (index)
        {
            case 0:
                break;
            case 1:

                Vector2 originalHPosition = rt.anchoredPosition;
                rt.anchorMax = new Vector2(1, 0);
                rt.anchorMin = new Vector2(1, 0);
                rt.anchoredPosition = new Vector2(-originalHPosition.x, originalHPosition.y);
                barFillImage.rectTransform.localScale = new Vector3(-1,1, 1);

                break;
        }
    }

    void UpdateHeatValue(object sender, float newValue)
    {
        barFillImage.fillAmount = newValue / manager.MaxHeatValue;
    }

    void OnDisable()
    {
        manager.OnHeatValueChanged -= UpdateHeatValue;
    }
}