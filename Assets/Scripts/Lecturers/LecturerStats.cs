using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LECTURER_DESIRES
{
    NONE = -1,
    RESEARCH,
    TEACHING
}

public enum LECTURER_TRAITS
{
    LAZY,
    HAPPY,
    SAD,
    HARDWORKING
}

public enum LECTURER_LOYALTY
{
    LOYAL,
    INDIFFERENT,
    DISLOYAL
}

public class LecturerStats : MonoBehaviour
{
    public string lecturerName = "Lecturer";
    public Sprite profilePicture;
    public LECTURER_DESIRES lecturerDesire = LECTURER_DESIRES.TEACHING;
    public List<LECTURER_TRAITS> lecturerTraits;
    public LECTURER_LOYALTY lecturerLoyalty = LECTURER_LOYALTY.INDIFFERENT;

    public void RandomiseStats()
    {
        lecturerTraits = new List<LECTURER_TRAITS>(Random.Range(1, 5));
        for (int j = 0; j < lecturerTraits.Capacity; j++)
        {
            lecturerTraits.Add((LECTURER_TRAITS)Random.Range(0, 4));
        }

        lecturerLoyalty = (LECTURER_LOYALTY)Random.Range(0, 3);
        lecturerDesire = (LECTURER_DESIRES)Random.Range(0, 2);
    }
}
