using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime
{
    public int minutes = 0;
    public int hours = 5;
    public int day = 0;
    public int month = 0;

    public static int MINUTES_IN_HOUR = 100;
    public static int HOURS_IN_DAY = 10;
    public static int DAYS_IN_MONTH = 10;

    public delegate void OnMinuteIncrementDelegate();
    public event OnHourIncrementDelegate OnMinuteIncrement;

    public delegate void OnHourIncrementDelegate();
    public event OnHourIncrementDelegate OnHourIncrement;

    public delegate void OnDayIncrementDelegate();
    public event OnDayIncrementDelegate OnDayIncrement;

    public delegate void OnMonthIncrementDelegate();
    public event OnMonthIncrementDelegate OnMonthIncrement;

    public void AddMinutes(int minsToAdd)
    {
        if (minutes + minsToAdd >= MINUTES_IN_HOUR)
        {
            AddMinutes(minsToAdd - MINUTES_IN_HOUR);
            AddHours(1);
        } else
        {
            minutes += minsToAdd;
            if (OnMinuteIncrement != null)
                OnMinuteIncrement();
        }
    }

    public void AddHours(int hoursToAdd)
    {
        if (hours + hoursToAdd >= HOURS_IN_DAY)
        {
            AddHours(hoursToAdd - HOURS_IN_DAY);
            AddDays(1);
        } else
        {
            hours += hoursToAdd;
            if (OnHourIncrement != null)
                OnHourIncrement();
        }
    }

    public void AddDays(int daysToAdd)
    {
        if (day + daysToAdd > DAYS_IN_MONTH)
        {
            AddDays(daysToAdd - DAYS_IN_MONTH);
            AddMonths(1);
        }
        else
        {
            day += daysToAdd;
            if (OnDayIncrement != null)
                OnDayIncrement();
        }
    }

    public void AddMonths(int monthsToAdd)
    {
        month += monthsToAdd;
        OnMonthIncrement();
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
        for (int i = 0; i < GameTime.HOURS_IN_DAY; i++)
        {
            if (i < 3 || i > 9) { timeslotsSetup.Add(TIMESLOT.SLEEPING); }
            else if (i == 3 || i == 7 || i == 8) { timeslotsSetup.Add(TIMESLOT.TEACHING); }
            else if (i == 4 || i == 5) { timeslotsSetup.Add(TIMESLOT.EATING); }
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
