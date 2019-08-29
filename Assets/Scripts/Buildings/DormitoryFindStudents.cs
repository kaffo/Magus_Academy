using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dormitory))]
public class DormitoryFindStudents : MonoBehaviour
{
    private Dormitory myDormitoryScript
    {
        get
        {
            return gameObject.GetComponent<Dormitory>();
        }
    }

    private void Start()
    {
        GameObject enrolledStudentsObject = StudentPool.Instance.enrolledGameObject;
        List<StudentFindAccommodation> studentAccomidations = new List<StudentFindAccommodation>(enrolledStudentsObject.GetComponentsInChildren<StudentFindAccommodation>());

        foreach (StudentFindAccommodation currentStudentAccomidation in studentAccomidations)
        {
            if (currentStudentAccomidation.myDormitory == null && myDormitoryScript.AccommodationSpaceRemaining() > 0)
            {
                myDormitoryScript.boardingStudents.Add(currentStudentAccomidation.myStudentStats);
                currentStudentAccomidation.myDormitory = myDormitoryScript;
            }
            if (myDormitoryScript.AccommodationSpaceRemaining() <= 0)
            {
                return;
            }
        }
    }
}
