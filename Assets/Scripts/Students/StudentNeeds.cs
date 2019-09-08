using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MageAcademy;

public class StudentNeeds : MonoBehaviour
{
    public float studentHunger = 0f;
    public float studentBoredom = 0f;
    public float studentSleepiness = 0f;

    public bool isBusy = false;
    public bool isEating = false;
    public bool isSleeping = false;

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
        if (isEating)
        {
            studentHunger = Mathf.Clamp(studentHunger - Constants.STUDENT_HUNGER_DECREASE_RATE, 0f, 1f);
        } else
        {
            studentHunger = Mathf.Clamp(studentHunger + Constants.STUDENT_HUNGER_INCREASE_RATE, 0f, 1f);
        }
        
        if (isBusy)
        {
            studentBoredom = Mathf.Clamp(studentBoredom - Constants.STUDENT_BOREDOM_DECREASE_RATE, 0f, 1f);
        } else
        {
            studentBoredom = Mathf.Clamp(studentBoredom + Constants.STUDENT_BOREDOM_INCREASE_RATE, 0f, 1f);
        }
        
        if (isSleeping)
        {
            studentSleepiness = Mathf.Clamp(studentSleepiness - Constants.STUDENT_SLEEPINESS_DECREASE_RATE, 0f, 1f);
        } else
        {
            studentSleepiness = Mathf.Clamp(studentSleepiness + Constants.STUDENT_SLEEPINESS_INCREASE_RATE, 0f, 1f);
        }
        
    }
}
