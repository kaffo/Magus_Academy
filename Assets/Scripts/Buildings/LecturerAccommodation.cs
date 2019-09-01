using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LecturerAccommodation : Building
{
    public int capacity = 2;

    public HashSet<LecturerStats> boardingLecturers;

    protected override void Awake()
    {
        boardingLecturers = new HashSet<LecturerStats>();
        base.Awake();
    }

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingLecturers.Count;
    }
}
