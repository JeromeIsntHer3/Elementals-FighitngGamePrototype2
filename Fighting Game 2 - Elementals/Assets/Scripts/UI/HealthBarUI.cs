using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthImg;
    [SerializeField] BaseCharacterHealth owner;

    void Start()
    {
        
    }

    void OnEnable()
    {
        owner.OnDamaged += OnDamaged;
    }

    void OnDisable()
    {
        owner.OnDamaged -= OnDamaged;
    }

    void OnDamaged(object sender, float currHealth)
    {
        healthImg.DOFillAmount(currHealth, GameManager.HealthAnimationDuration);
    }

    void Update()
    {
        
    }
}