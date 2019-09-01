using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitory : MonoBehaviour
{
    public int capacity = 10;

    public List<StudentStats> boardingStudents;
    private HashSet<StudentMovement> studentsInside;
    private GameTime gameTime;

    private void Awake()
    {
        studentsInside = new HashSet<StudentMovement>();
        gameTime = TimeManager.Instance.currentTime;
    }

    public void StudentEnter(StudentMovement studentMoveScript)
    {
        studentsInside.Add(studentMoveScript);
        studentMoveScript.HideStudent();
    }

    public void StudentExit(StudentMovement studentMoveScript)
    {
        if (studentsInside.Contains(studentMoveScript))
        {
            studentsInside.Remove(studentMoveScript);
            studentMoveScript.ShowStudent();
        }
        else
        {
            Debug.LogWarning("Student " + studentMoveScript.name + " tried to exit " + gameObject.name + " and failed");
        }
    }

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingStudents.Count;
    }
}
