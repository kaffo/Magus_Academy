using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitory : Building
{
    public int capacity = 10;

    public List<StudentStats> boardingStudents;

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingStudents.Count;
    }
}
