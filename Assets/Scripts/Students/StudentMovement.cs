using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

enum STUDENT_BEHAVIOUR
{
    None = -1,
    Sleeping,
    Wandering,
    Learning,
    Mischief
}

[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(StudentFindAccommodation))]
public class StudentMovement : MonoBehaviour
{
    public int xBound = 30;
    public int yBound = 30;

    private PolyNavAgent myPolyNavAgent
    {
        get
        {
            return gameObject.GetComponent<PolyNavAgent>();
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
        if (myPolyNavAgent.hasPath) { return; }
        switch (currentBehaviour)
        {
            case STUDENT_BEHAVIOUR.Wandering:
                WanderNearby();
                break;
            case STUDENT_BEHAVIOUR.Sleeping:
                ReturnToDormitory();
                break;
            default:
                WanderNearby();
                break;
        }
    }

    private void OnHourChange()
    {
        if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.SLEEPING && currentBehaviour != STUDENT_BEHAVIOUR.Sleeping)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Sleeping;
            myPolyNavAgent.Stop();
        } else if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.SLEEPING && currentBehaviour == STUDENT_BEHAVIOUR.Sleeping)
        {
            currentBehaviour = STUDENT_BEHAVIOUR.Wandering;
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
        Vector2 destination = new Vector2(dormLocation.x + 1, dormLocation.y - 2);

        myPolyNavAgent.SetDestination(destination);
    }

    private void ReachedDestination()
    {
        switch(currentBehaviour)
        {
            case STUDENT_BEHAVIOUR.Wandering:
                WanderNearby();
                break;
            case STUDENT_BEHAVIOUR.Sleeping:
                myStudentAccommodationScript.myDormitory.StudentEnter(gameObject);
                break;
            default:
                WanderNearby();
                break;
        }
    }
}
