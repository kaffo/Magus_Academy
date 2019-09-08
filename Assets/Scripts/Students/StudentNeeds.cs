using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MageAcademy;

public class StudentNeeds : MonoBehaviour
{
    public float studentHunger = 1f;
    public float studentBoredom = 0f;
    public float studentSleepiness = 0f;

    private GameTime gameTime;

    private void Awake()
    {
        gameTime = TimeManager.Instance.currentTime;
    }

    void OnEnable()
    {
        gameTime.OnMinuteIncrement += OnMinuteChange;
    }

    void OnDisable()
    {
        gameTime.OnMinuteIncrement -= OnMinuteChange;
    }

    private void OnMinuteChange()
    {
        studentHunger = Mathf.Clamp(studentHunger - Constants.STUDENT_HUNGER_RATE, 0f, 1f);
        studentBoredom = Mathf.Clamp(studentBoredom + Constants.STUDENT_BOREDOM_RATE, 0f, 1f);
        studentSleepiness = Mathf.Clamp(studentSleepiness + Constants.STUDENT_SLEEPINESS_RATE, 0f, 1f);
    }
}
