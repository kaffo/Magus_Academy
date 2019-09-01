using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class FoodHall : Building
{
    public int capacity = 10;

    private HashSet<PolyNavAgent> reserveList;

    protected override void Awake()
    {
        reserveList = new HashSet<PolyNavAgent>();
        base.Awake();
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

    public override void StudentExit(StudentMovement studentMoveScript)
    {
        if (studentsInside.Contains(studentMoveScript))
        {
            CancelReservation(studentMoveScript.myPolyNavAgent);
            base.StudentExit(studentMoveScript);
        }
    }

    public override void LecturerExit(LecturerMovement lecturerMoveScript)
    {
        if (lecturersInside.Contains(lecturerMoveScript))
        {
            CancelReservation(lecturerMoveScript.myPolyNavAgent);
            base.LecturerExit(lecturerMoveScript);
        }
    }
}
