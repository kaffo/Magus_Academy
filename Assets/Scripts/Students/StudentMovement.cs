﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

enum STUDENT_BEHAVIOUR
{
    None = -1,
    Sleeping,
    Eating,
    Wandering,
    Learning,
    Mischief
}

[RequireComponent(typeof(StudentStats))]
[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(StudentFindAccommodation))]
public class StudentMovement : MonoBehaviour
{
    public int xBound = 30;
    public int yBound = 30;

    public PolyNavAgent myPolyNavAgent
    {
        get
        {
            return gameObject.GetComponent<PolyNavAgent>();
        }
    }

    private StudentStats myStudentStats
    {
        get
        {
            return gameObject.GetComponent<StudentStats>();
        }
    }

    private StudentFindAccommodation myStudentAccommodationScript
    {
        get
        {
            return gameObject.GetComponent<StudentFindAccommodation>();
        }
    }

    private GameTime gameTime;
    private STUDENT_BEHAVIOUR currentBehaviour = STUDENT_BEHAVIOUR.Wandering;

    private Classroom currentClassroom = null;
    private FoodHall currentFoodhall = null;

    private void Awake()
    {
        gameTime = TimeManager.Instance.currentTime;
    }

    void OnEnable()
    {
        myPolyNavAgent.OnDestinationReached += ReachedDestination;
        //myPolyNavAgent.OnDestinationInvalid += MoveRandom;
        gameTime.OnHourIncrement += OnHourChange;

        if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.SLEEPING) { currentBehaviour = STUDENT_BEHAVIOUR.Sleeping; }
        else { currentBehaviour = STUDENT_BEHAVIOUR.Wandering; }
    }

    void OnDisable()
    {
        myPolyNavAgent.OnDestinationReached -= ReachedDestination;
        //myPolyNavAgent.OnDestinationInvalid -= MoveRandom;
        gameTime.OnHourIncrement -= OnHourChange;
    }

    private void Update()
    {
        if (!myPolyNavAgent.enabled || myPolyNavAgent.hasPath) { return; }
        switch (currentBehaviour)
        {
            case STUDENT_BEHAVIOUR.Wandering:
                WanderNearby();
                break;
            case STUDENT_BEHAVIOUR.Sleeping:
                ReturnToDormitory();
                break;
            case STUDENT_BEHAVIOUR.Learning:
                FindAndPathToClassroom();
                break;
            case STUDENT_BEHAVIOUR.Eating:
                FindAndPathToFoodhall();
                break;
            default:
                WanderNearby();
                break;
        }
    }

    public void HideStudent()
    {
        myPolyNavAgent.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ShowStudent()
    {
        myPolyNavAgent.enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnHourChange()
    {
        // Inside a building
        if (!myPolyNavAgent.enabled)
        {
            // Was learning, now leaving
            if (currentBehaviour == STUDENT_BEHAVIOUR.Learning && TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.TEACHING && currentClassroom)
            {
                currentClassroom.StudentExit(this);
            }
            // Was sleeping, now leaving
            else if (currentBehaviour == STUDENT_BEHAVIOUR.Sleeping && TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.SLEEPING && myStudentAccommodationScript.myDormitory)
            {
                myStudentAccommodationScript.myDormitory.StudentExit(this);
            }
            // Was eating, now leaving
            else if (currentBehaviour == STUDENT_BEHAVIOUR.Eating && TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.EATING && currentFoodhall)
            {
                currentFoodhall.StudentExit(this);
            }
        }

        // Time to sleep
        if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.SLEEPING && currentBehaviour != STUDENT_BEHAVIOUR.Sleeping)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Sleeping;
            myPolyNavAgent.Stop();
        }
        // Nothing to do
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.NONE && currentBehaviour != STUDENT_BEHAVIOUR.Wandering)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Wandering;
            myPolyNavAgent.Stop();
        }
        // Time to go to class
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.TEACHING && currentBehaviour != STUDENT_BEHAVIOUR.Learning)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Learning;
            myPolyNavAgent.Stop();
        }
        // Time to eat
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.EATING && currentBehaviour != STUDENT_BEHAVIOUR.Eating)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Eating;
            myPolyNavAgent.Stop();
        }
    }

    private void WanderNearby()
    {
        myPolyNavAgent.maxSpeed = 1;
        Vector2 destination = new Vector2(
            Mathf.Clamp(Random.Range((int)transform.position.x - 15, (int)transform.position.x + 15), -xBound, xBound),
            Mathf.Clamp(Random.Range((int)transform.position.y - 15, (int)transform.position.y + 15), -yBound, yBound));

        myPolyNavAgent.SetDestination(destination);
    }

    private void ReturnToDormitory()
    {
        if (!myStudentAccommodationScript.myDormitory)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 dormLocation = myStudentAccommodationScript.myDormitory.transform.position;
        Vector2 destination = new Vector2(dormLocation.x + 1, dormLocation.y);

        myPolyNavAgent.SetDestination(destination);
    }

    private void FindAndPathToClassroom()
    {
        currentClassroom = BuildingPlacement.Instance.FindClassroom(myStudentStats.studentDesire);
        if (!currentClassroom)
        {
            Debug.LogWarning("Can't find a classroom for " + myStudentStats.studentDesire.ToString());
            currentBehaviour = STUDENT_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 classLocation = currentClassroom.transform.position;
        Vector2 destination = new Vector2(classLocation.x + 2, classLocation.y);

        myPolyNavAgent.SetDestination(destination);
    }

    private void FindAndPathToFoodhall()
    {
        if (!currentFoodhall)
        {
            List<FoodHall> allFoodhalls = new List<FoodHall>(BuildingPlacement.Instance.corePoolObject.GetComponentsInChildren<FoodHall>());
            foreach (FoodHall foodHall in allFoodhalls)
            {
                if (foodHall.ReservePlace(myPolyNavAgent))
                {
                    currentFoodhall = foodHall;
                    break;
                }
            }
        }

        if (!currentFoodhall)
        {
            Debug.LogWarning("Student couldn't find a foodhall to eat in");
            currentBehaviour = STUDENT_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 foodLocation = currentFoodhall.transform.position;
        Vector2 destination = new Vector2(foodLocation.x, foodLocation.y);

        myPolyNavAgent.SetDestination(destination);
    }

    private void ReachedDestination()
    {
        switch (currentBehaviour)
        {
            case STUDENT_BEHAVIOUR.Wandering:
                WanderNearby();
                break;
            case STUDENT_BEHAVIOUR.Sleeping:
                myStudentAccommodationScript.myDormitory.StudentEnter(this);
                break;
            case STUDENT_BEHAVIOUR.Learning:
                if (currentClassroom)
                    currentClassroom.StudentEnter(this);
                break;
            case STUDENT_BEHAVIOUR.Eating:
                if (currentFoodhall)
                    currentFoodhall.StudentEnter(this);
                break;
            default:
                WanderNearby();
                break;
        }
    }
}
