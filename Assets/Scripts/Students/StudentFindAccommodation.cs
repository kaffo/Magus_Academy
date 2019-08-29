using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
public class StudentFindAccommodation : MonoBehaviour
{
    public SmallDormitory myDormitory;
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
        List<SmallDormitory> dormitories = new List<SmallDormitory>(dormitoryObject.GetComponentsInChildren<SmallDormitory>());

        foreach (SmallDormitory currentDormitory in dormitories)
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
