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
            OnMinuteIncrement();
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
            OnHourIncrement();
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
            OnDayIncrement();
        }
    }

    public void AddMonths(int monthsToAdd)
    {
        month += monthsToAdd;
    }
}

public enum TIMESLOT
{
    NONE = -1,
    SLEEPING,
    TEACHING,
    EATING
}

public class TimeManager : Singleton<TimeManager>
{
    public float rateOfTime = 0.1f;
    public List<TIMESLOT> timeslotsSetup = new List<TIMESLOT>();
    [HideInInspector()]
    public GameTime currentTime = new GameTime();

    private Coroutine timePassCoroutine;

    private void Awake()
    {
        for (int i = 0; i < 24; i++)
        {
            if (i < 7 || i > 19) { timeslotsSetup.Add(TIMESLOT.SLEEPING); }
            else if (i == 11 || i == 15 || i == 16) { timeslotsSetup.Add(TIMESLOT.TEACHING); }
            else if (i == 12 || i == 13) { timeslotsSetup.Add(TIMESLOT.EATING); }
            else { timeslotsSetup.Add(TIMESLOT.NONE); }
        }
    }

    private void OnEnable()
    {
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

    public TIMESLOT GetCurrentTimeslot()
    {
        return timeslotsSetup[currentTime.hours];
    }
}
