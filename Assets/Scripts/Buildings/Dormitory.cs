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

    private void OnEnable()
    {
        gameTime.OnHourIncrement += OnHourChange;
    }

    private void OnDisable()
    {
        gameTime.OnHourIncrement += OnHourChange;
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

    private void OnHourChange()
    {
        if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.SLEEPING && studentsInside.Count > 0)
        {
            List<StudentMovement> studentsToRemove = new List<StudentMovement>(studentsInside);
            foreach (StudentMovement studentGameObject in studentsToRemove)
            {
                StudentExit(studentGameObject);
            }
        } 
    }

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingStudents.Count;
    }
}
