using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateUITracker : MonoBehaviour
{
    public Text dateText;

    private GameTime gameTime;
    private string templateText = "Day {0}  Month {1}   Year {2}";

    private void Start()
    {
        if (dateText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        gameTime = TimeManager.Instance.currentTime;
        gameTime.OnDayIncrement += UpdateDateText;
        gameTime.OnMonthIncrement += UpdateDateText;
        UpdateDateText();
    }

    private void UpdateDateText()
    {
        dateText.text = string.Format(templateText, gameTime.day, gameTime.month, 1);
    }
}
