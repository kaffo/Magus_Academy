using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditManager : Singleton<AuditManager>
{
    private int currentDeductions = 0;

    private List<GameObject> lastKnownStudents;
    private List<GameObject> lastKnownLecturers;
    private List<GameObject> lastKnownBuildings;

    private void Start()
    {
        lastKnownStudents = new List<GameObject>();
        lastKnownLecturers = new List<GameObject>();
        lastKnownBuildings = new List<GameObject>();
    }

    // TODO make this smarter
    public void ProvideAuditUpdate(List<GameObject> students, List<GameObject> lecturers, List<GameObject> buildings)
    {
        lastKnownStudents = students;
        lastKnownLecturers = lecturers;
        lastKnownBuildings = buildings;
        CheckForInfractions();
    }

    private void CheckForInfractions()
    {
        int maxStudents = StudentPool.Instance.currentStudentCapacity;
        int maxLecturers = LecturerManager.Instance.currentLecturerCapacity;
        int maxBuildings = 999; // Not implemented yet

        // Reset current deductions from last year
        currentDeductions = 0;

        if (lastKnownStudents.Count > maxStudents)
        {
            // 500 fine for each student over cap
            currentDeductions += (500 * (lastKnownStudents.Count - maxStudents));
        }

        if (lastKnownLecturers.Count > maxLecturers)
        {
            // 1000 fine for each lecturer over cap
            currentDeductions += (1000 * (lastKnownLecturers.Count - maxLecturers));
        }

        // TODO Split this into building types
        if (lastKnownBuildings.Count > maxBuildings)
        {
            // 750 fine for each building over cap
            currentDeductions += (750 * (lastKnownBuildings.Count - maxBuildings));
        }

        Debug.Log($"Current Deductions: {currentDeductions}");
    }
}
