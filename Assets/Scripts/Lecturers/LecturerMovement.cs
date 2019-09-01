using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

enum LECTURER_BEHAVIOUR
{
    None = -1,
    Sleeping,
    Eating,
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

    public PolyNavAgent myPolyNavAgent
    {
        get
        {
            return gameObject.GetComponent<PolyNavAgent>();
        }
    }

    private LecturerStats myLecturerStats
    {
        get
        {
            return gameObject.GetComponent<LecturerStats>();
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
                ReturnToAccomodation();
                break;
            case LECTURER_BEHAVIOUR.Teaching:
                FindAndPathToClassroom();
                break;
            case LECTURER_BEHAVIOUR.Eating:
                FindAndPathToFoodhall();
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
        // Inside a building
        if (!myPolyNavAgent.enabled)
        {
            // Was teaching, time to leave
            if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.TEACHING && currentBehaviour == LECTURER_BEHAVIOUR.Teaching  && myLecturerAccommodationScript.myClassroom)
            {
                myLecturerAccommodationScript.myClassroom.LecturerExit(this);
            }
            // Was teaching, time to leave
            else if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.SLEEPING && currentBehaviour == LECTURER_BEHAVIOUR.Sleeping && myLecturerAccommodationScript.myAccommodation)
            {
                myLecturerAccommodationScript.myAccommodation.LecturerExit(this);
            }
            // Was eating, time to leave
            else if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.EATING && currentBehaviour == LECTURER_BEHAVIOUR.Eating && currentFoodhall)
            {
                currentFoodhall.LecturerExit(this);
            }
        }

        // Time to go to sleep
        if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.SLEEPING && currentBehaviour != LECTURER_BEHAVIOUR.Sleeping)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Sleeping;
            myPolyNavAgent.Stop();
        }
        // Nothing to do
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.NONE && currentBehaviour != LECTURER_BEHAVIOUR.Wandering)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Wandering;
            myPolyNavAgent.Stop();
        }
        // Time to go teach
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.TEACHING && currentBehaviour != LECTURER_BEHAVIOUR.Teaching)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Teaching;
            myPolyNavAgent.Stop();
        }
        // Time to go eat
        else if (TimeManager.Instance.GetCurrentTimeslot() == TIMESLOT.EATING && currentBehaviour != LECTURER_BEHAVIOUR.Eating)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Eating;
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

    private void ReturnToAccomodation()
    {
        if (!myLecturerAccommodationScript.myAccommodation)
        {
            currentBehaviour = LECTURER_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 dormLocation = myLecturerAccommodationScript.myAccommodation.transform.position;
        Vector2 destination = new Vector2(dormLocation.x + 1, dormLocation.y);

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
        Vector2 destination = new Vector2(classLocation.x + 2, classLocation.y );

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
            Debug.LogWarning("Lecturer couldn't find a foodhall to eat in");
            currentBehaviour = LECTURER_BEHAVIOUR.Wandering;
            return;
        }

        myPolyNavAgent.maxSpeed = 4;
        Vector3 foodLocation = currentFoodhall.transform.position;
        Vector2 destination = new Vector2(foodLocation.x , foodLocation.y);

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
                if (myLecturerAccommodationScript.myAccommodation)
                    myLecturerAccommodationScript.myAccommodation.LecturerEnter(this);
                break;
            case LECTURER_BEHAVIOUR.Teaching:
                if (myLecturerAccommodationScript.myClassroom)
                    myLecturerAccommodationScript.myClassroom.LecturerEnter(this);
                break;
            case LECTURER_BEHAVIOUR.Eating:
                if (currentFoodhall)
                    currentFoodhall.LecturerEnter(this);
                break;
            default:
                WanderNearby();
                break;
        }
    }
}
