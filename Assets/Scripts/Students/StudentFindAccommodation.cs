using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
public class StudentFindAccommodation : MonoBehaviour
{
    public Dormitory myDormitory;
    public StudentStats myStudentStats
    {
        get
        {
            return gameObject.GetComponent<StudentStats>();
        }
    }

    private void Start()
    {
        GameObject dormitoryObject = BuildingPlacement.Instance.domitoryPoolObject;
        List<Dormitory> dormitories = new List<Dormitory>(dormitoryObject.GetComponentsInChildren<Dormitory>());

        foreach (Dormitory currentDormitory in dormitories)
        {
            if (currentDormitory.AccommodationSpaceRemaining() > 0)
            {
                currentDormitory.boardingStudents.Add(myStudentStats);
                myDormitory = currentDormitory;
                return;
            }
        }
        Debug.Log("Couldn't find a dormitory for " + gameObject.name);
    }
}
