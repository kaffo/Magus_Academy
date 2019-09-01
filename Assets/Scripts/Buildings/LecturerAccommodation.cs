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

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingLecturers.Count;
    }
}
