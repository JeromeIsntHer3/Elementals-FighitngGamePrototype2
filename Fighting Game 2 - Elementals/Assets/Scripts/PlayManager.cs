using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float countdownDuration;

    bool startCountdown;
    float currentTime;

    void Start()
    {
        currentTime = countdownDuration;
    }

    void Update()
    {
        if (!startCountdown) return;
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        SetTimerText();
    }

    void SetTimerText()
    {
        countdownText.text = currentTime.ToString("0");
    }
}