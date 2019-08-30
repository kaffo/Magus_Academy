using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class FoodHall : MonoBehaviour
{
    public int capacity = 10;

    private HashSet<PolyNavAgent> reserveList;
    private HashSet<StudentMovement> studentsInside;
    private HashSet<LecturerMovement> lecturersInside;
    private GameTime gameTime;

    private void Awake()
    {
        reserveList = new HashSet<PolyNavAgent>();
        studentsInside = new HashSet<StudentMovement>();
        lecturersInside = new HashSet<LecturerMovement>();
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

    public bool ReservePlace(PolyNavAgent agentToReserve)
    {
        if (reserveList.Count < capacity)
        {
            return reserveList.Add(agentToReserve);
        } else
        {
            return false;
        }
    }

    public void CancelReservation(PolyNavAgent agentToRemove)
    {
        if (reserveList.Contains(agentToRemove))
            reserveList.Remove(agentToRemove);
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
            CancelReservation(studentMoveScript.myPolyNavAgent);
            studentsInside.Remove(studentMoveScript);
            studentMoveScript.ShowStudent();
        }
        else
        {
            Debug.LogWarning("Student " + studentMoveScript.name + " tried to exit " + gameObject.name + " and failed");
        }
    }

    public void LecturerEnter(LecturerMovement lecturerMoveScript)
    {
        lecturersInside.Add(lecturerMoveScript);
        lecturerMoveScript.HideLecturer();
    }

    public void LecturerExit(LecturerMovement lecturerMoveScript)
    {
        if (lecturersInside.Contains(lecturerMoveScript))
        {
            CancelReservation(lecturerMoveScript.myPolyNavAgent);
            lecturersInside.Remove(lecturerMoveScript);
            lecturerMoveScript.ShowLecturer();
        }
        else
        {
            Debug.LogWarning("Lecturer " + lecturerMoveScript.name + " tried to exit " + gameObject.name + " and failed");
        }
    }

    private void OnHourChange()
    {
        if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.EATING && studentsInside.Count > 0)
        {
            List<StudentMovement> studentsToRemove = new List<StudentMovement>(studentsInside);
            foreach (StudentMovement studentGameObject in studentsToRemove)
            {
                StudentExit(studentGameObject);
            }
        }
        if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.EATING && lecturersInside.Count > 0)
        {
            List<LecturerMovement> lecturersToRemove = new List<LecturerMovement>(lecturersInside);
            foreach (LecturerMovement lecturerGameObject in lecturersToRemove)
            {
                LecturerExit(lecturerGameObject);
            }
        }
    }
}
