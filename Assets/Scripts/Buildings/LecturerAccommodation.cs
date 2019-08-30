using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LecturerAccommodation : MonoBehaviour
{
    public int capacity = 2;

    public List<LecturerStats> boardingLecturers;
    private HashSet<LecturerMovement> lecturersInside;
    private GameTime gameTime;

    private void Awake()
    {
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

    public void LecturerEnter(LecturerMovement lecturerMoveScript)
    {
        lecturersInside.Add(lecturerMoveScript);
        lecturerMoveScript.HideLecturer();
    }

    public void LecturerExit(LecturerMovement lecturerMoveScript)
    {
        if (lecturersInside.Contains(lecturerMoveScript))
        {
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
        if (TimeManager.Instance.GetCurrentTimeslot() != TIMESLOT.SLEEPING && lecturersInside.Count > 0)
        {
            List<LecturerMovement> lecturersToRemove = new List<LecturerMovement>(lecturersInside);
            foreach (LecturerMovement lecturerGameObject in lecturersToRemove)
            {
                LecturerExit(lecturerGameObject);
            }
        } 
    }

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingLecturers.Count;
    }
}
