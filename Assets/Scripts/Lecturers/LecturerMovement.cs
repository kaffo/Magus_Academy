using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

enum LECTURER_BEHAVIOUR
{
    None = -1,
    Sleeping,
    Wandering,
    Teaching,
    Research
}

[RequireComponent(typeof(LecturerStats))]
[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(LecturerFindAccommodationAndClassroom))]
public class LecturerMovement : MonoBehaviour
{
    public int xBound = 30;
    public int yBound = 30;

    private LecturerStats myLecturerStats
    {
        get
        {
            return gameObject.GetComponent<LecturerStats>();
        }
    }

    private PolyNavAgent myPolyNavAgent
    {
        get
        {
            return gameObject.GetComponent<PolyNavAgent>();
        }
    }

    private LecturerFindAccommodationAndClassroom myLecturerAccommodationScript
    {
        get
        {
            return gameObject.GetComponent<LecturerFindAccommodationAndClassroom>();
        }
    }

    private GameTime gameTime;
    private LECTURER_BEHAVIOUR currentBehaviour = LECTURER_BEHAVIOUR.Wandering;

    private void Awake()
    {
        gameTime = TimeManager.Instance.currentTime;
    }

    void OnEnable()
    {
        myPolyNavAgent.OnDestinationReached += ReachedDestination;
        //myPolyNavAgent.OnDestinationInvalid += MoveRandom;
        gameTime.OnHourIncrement += OnHourChange;

        if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.SLEEPING) { currentBehaviour = LECTURER_BEHAVIOUR.Sleeping; }
        else { currentBehaviour = LECTURER_BEHAVIOUR.Wandering; }
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
            case LECTURER_BEHAVIOUR.Wandering:
                WanderNearby();
                break;
            case LECTURER_BEHAVIOUR.Sleeping:
                ReturnToDormitory();
                break;
            case LECTURER_BEHAVIOUR.Teaching:
                FindAndPathToClassroom();
                break;
            default:
                WanderNearby();
                break;
        }
    }

    public void HideLecturer()
    {
        myPolyNavAgent.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ShowLecturer()
    {
        myPolyNavAgent.enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnHourChange()
    {
        if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.SLEEPING && currentBehaviour != LECTURER_BEHAVIOUR.Sleeping)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Sleeping;
            myPolyNavAgent.Stop();
        }
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.NONE && currentBehaviour != LECTURER_BEHAVIOUR.Wandering)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Wandering;
            myPolyNavAgent.Stop();
        }
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.TEACHING && currentBehaviour != LECTURER_BEHAVIOUR.Teaching)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Teaching;
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
        if (!myLecturerAccommodationScript.myDormitory)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 dormLocation = myLecturerAccommodationScript.myDormitory.transform.position;
        Vector2 destination = new Vector2(dormLocation.x + 1, dormLocation.y - 2);

        myPolyNavAgent.SetDestination(destination);
    }

    private void FindAndPathToClassroom()
    {
        if (!myLecturerAccommodationScript.myClassroom)
        {
            Debug.LogWarning("Lecturer doesn't have a classroom");
            currentBehaviour = LECTURER_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 classLocation = myLecturerAccommodationScript.myClassroom.transform.position;
        Vector2 destination = new Vector2(classLocation.x + 2, classLocation.y - 4);

        myPolyNavAgent.SetDestination(destination);
    }

    private void ReachedDestination()
    {
        switch(currentBehaviour)
        {
            case LECTURER_BEHAVIOUR.Wandering:
                WanderNearby();
                break;
            case LECTURER_BEHAVIOUR.Sleeping:
                break;
            case LECTURER_BEHAVIOUR.Teaching:
                if (myLecturerAccommodationScript.myClassroom)
                    myLecturerAccommodationScript.myClassroom.LecturerEnter(this);
                break;
            default:
                WanderNearby();
                break;
        }
    }
}
