using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthImg;

    public void OnHealthDepleted(object sender, float currHealth)
    {
        healthImg.DOFillAmount(currHealth, GameManager.HealthChangeAnimationDuration);
    }
}