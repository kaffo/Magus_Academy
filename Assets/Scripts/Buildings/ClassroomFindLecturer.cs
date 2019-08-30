using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Classroom))]
public class ClassroomFindLecturer : MonoBehaviour
{
    private Classroom myClassroomScript
    {
        get
        {
            return gameObject.GetComponent<Classroom>();
        }
    }

    private void Start()
    {
        if (myClassroomScript.myLecturer != null) { return; }

        GameObject enrolledLecturersObject = LecturerManager.Instance.hiredPoolGameObject;
        List<LecturerFindAccommodationAndClassroom> lecturerClassrooms = new List<LecturerFindAccommodationAndClassroom>(enrolledLecturersObject.GetComponentsInChildren<LecturerFindAccommodationAndClassroom>());

        foreach (LecturerFindAccommodationAndClassroom currentLecturerClassroom in lecturerClassrooms)
        {
            if (currentLecturerClassroom.myClassroom == null)
            {
                myClassroomScript.myLecturer = currentLecturerClassroom.myLecturerMovement;
                currentLecturerClassroom.myClassroom = myClassroomScript;
                return;
            }
        }
    }
}
