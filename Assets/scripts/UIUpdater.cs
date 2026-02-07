using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    public TMP_Text bugCounterField;
    public TMP_Text timer;
    public int bugCount;
    private float timeExhausted = 0f;

    void Start()
    {
        UpdateBugCounter();
    }

    public void UpdateBugCounter()
    {
        bugCounterField.text = "Bugs Left: " + bugCount;
    }

    public void Update()
    {
        timeExhausted += Time.deltaTime;
        DisplayTime(timeExhausted);
    } 
    
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timer.text = string.Format("{0:00}", minutes, seconds);
    }
}
