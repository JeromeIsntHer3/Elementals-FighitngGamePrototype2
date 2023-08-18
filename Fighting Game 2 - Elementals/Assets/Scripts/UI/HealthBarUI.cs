using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthImg;

    public void OnHealthDepleted(object sender, float currHealth)
    {
        healthImg.DOFillAmount(currHealth, GameManager.HealthAnimationDuration);
    }
}