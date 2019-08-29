using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeClockTracker : MonoBehaviour
{
    public Text clockText;

    private GameTime gameTimeScript;

    private void Awake()
    {
        if (clockText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        gameTimeScript = TimeManager.Instance.currentTime;
        gameTimeScript.OnMinuteIncrement += UpdateClock;
        gameTimeScript.OnHourIncrement += UpdateClock;
        gameTimeScript.OnDayIncrement += UpdateClock;
    }

    private void UpdateClock()
    {
        int mins = gameTimeScript.minutes;
        string minsString = mins.ToString();
        if (mins < 10) { minsString = '0' + minsString; }
        clockText.text = gameTimeScript.hours.ToString() + ":" + minsString;
    }
}
