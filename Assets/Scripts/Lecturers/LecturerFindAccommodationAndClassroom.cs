using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LecturerStats))]
[RequireComponent(typeof(LecturerMovement))]
public class LecturerFindAccommodationAndClassroom : MonoBehaviour
{
    public Dormitory myDormitory;
    public Classroom myClassroom;
    private LecturerStats myLecturerStats
    {
        get
        {
            return gameObject.GetComponent<LecturerStats>();
        }
    }
    private LecturerMovement myLecturerMovement
    {
        get
        {
            return gameObject.GetComponent<LecturerMovement>();
        }
    }

    private void Start()
    {
        GameObject classroomPool = BuildingPlacement.Instance.classroomPoolObject;
        List<Classroom> classrooms = new List<Classroom>(classroomPool.GetComponentsInChildren<Classroom>());

        foreach (Classroom currentClassroom in classrooms)
        {
            if (currentClassroom.myLecturer == null)
            {
                currentClassroom.myLecturer = myLecturerMovement;
                myClassroom = currentClassroom;
                return;
            }
        }
        Debug.Log("Couldn't find a classroom for " + gameObject.name);
        //TODO Find accommodation
    }
}
