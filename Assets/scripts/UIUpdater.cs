using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public TMP_Text bugCounterField;
    public TMP_Text timer;
    public Button resetButton;
    public int bugCount = 3;
    private float timeExhausted = 0f;

    private bool timerRunning;

    void Start()
    {
        UpdateBugCounter();
        timerRunning = true;
    }

    public void UpdateBugCounter()
    {
        bugCounterField.text = "Bugs Left: " + bugCount;
    }

    private void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
            
            UpdateBugCounter();
        }
    }


    private void UpdateTimer()
    {
        timeExhausted += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeExhausted);
        timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
    
    public void StartTimer()
    {
        timeExhausted = 0;
        timerRunning = true;
    }
}
