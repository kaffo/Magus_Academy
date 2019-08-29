using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime
{
    public int minutes = 0;
    public int hours = 12;
    public int day = 0;
    public int month = 0;

    public delegate void OnMinuteIncrementDelegate();
    public event OnHourIncrementDelegate OnMinuteIncrement;

    public delegate void OnHourIncrementDelegate();
    public event OnHourIncrementDelegate OnHourIncrement;

    public delegate void OnDayIncrementDelegate();
    public event OnDayIncrementDelegate OnDayIncrement;

    public void AddMinutes(int minsToAdd)
    {
        if (minutes + minsToAdd >= 60)
        {
            AddMinutes(minsToAdd - 60);
            AddHours(1);
        } else
        {
            minutes += minsToAdd;
            if (minsToAdd > 0) { OnMinuteIncrement(); }
        }
    }

    public void AddHours(int hoursToAdd)
    {
        if (hours + hoursToAdd >= 24)
        {
            AddHours(hoursToAdd - 24);
            AddDays(1);
        } else
        {
            hours += hoursToAdd;
            if (hoursToAdd > 0) { OnHourIncrement(); }
        }
    }

    public void AddDays(int daysToAdd)
    {
        if (day + daysToAdd > 30)
        {
            AddDays(daysToAdd - 30);
            AddMonths(1);
        }
        else
        {
            day += daysToAdd;
            if (daysToAdd > 0) { OnDayIncrement(); }
        }
    }

    public void AddMonths(int monthsToAdd)
    {
        month += monthsToAdd;
    }
}

public class TimeManager : Singleton<TimeManager>
{
    public float rateOfTime = 0.1f;
    [HideInInspector()]
    public GameTime currentTime = new GameTime();

    private Coroutine timePassCoroutine;


    private void OnEnable()
    {
        if (timePassCoroutine == null)
            timePassCoroutine = StartCoroutine(PassTime());
    }

    private void OnDisable()
    {
        if (timePassCoroutine != null)
            StopCoroutine(timePassCoroutine);
    }

    private IEnumerator PassTime()
    {
        while (true)
        {
            currentTime.AddMinutes(1);
            yield return new WaitForSeconds(rateOfTime);
        }
    }
}
