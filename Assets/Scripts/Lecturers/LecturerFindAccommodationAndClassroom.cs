using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LecturerStats))]
[RequireComponent(typeof(LecturerMovement))]
public class LecturerFindAccommodationAndClassroom : MonoBehaviour
{
    public LecturerAccommodation myAccommodation;
    public Classroom myClassroom;
    [HideInInspector()]
    public LecturerMovement myLecturerMovement
    {
        get
        {
            return gameObject.GetComponent<LecturerMovement>();
        }
    }

    private LecturerStats myLecturerStats
    {
        get
        {
            return gameObject.GetComponent<LecturerStats>();
        }
    }

    private void Start()
    {
        if (!FindClassroom())
            Debug.Log("Couldn't find a classroom for " + gameObject.name);
        
        if (!FindAccommodation())
            Debug.Log("Couldn't find accommodation for " + gameObject.name);
    }

    private bool FindClassroom()
    {
        GameObject classroomPool = BuildingPlacement.Instance.classroomPoolObject;
        List<Classroom> classrooms = new List<Classroom>(classroomPool.GetComponentsInChildren<Classroom>());

        foreach (Classroom currentClassroom in classrooms)
        {
            if (currentClassroom.myLecturer == null && myLecturerStats.lecturerSkills[currentClassroom.classroomType] > 0.4)
            {
                currentClassroom.myLecturer = myLecturerMovement;
                myClassroom = currentClassroom;
                return true;
            }
        }
        return false;
    }

    private bool FindAccommodation()
    {
        GameObject accommodationPool = BuildingPlacement.Instance.corePoolObject;
        List<LecturerAccommodation> accommodations = new List<LecturerAccommodation>(accommodationPool.GetComponentsInChildren<LecturerAccommodation>());

        foreach (LecturerAccommodation currentAccommodation in accommodations)
        {
            if (currentAccommodation.AccommodationSpaceRemaining() > 0)
            {
                currentAccommodation.boardingLecturers.Add(myLecturerStats);
                myAccommodation = currentAccommodation;
                return true;
            }
        }
        return false;
    }
}
