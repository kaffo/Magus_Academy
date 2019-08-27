using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum STUDENT_DESIRES
{
    NONE = -1,
    NATURE,
    ELEMENTAL
}

public enum STUDENT_TRAITS
{
    LAZY,
    HAPPY,
    SAD,
    HARDWORKING
}

public enum STUDENT_LOYALTY
{
    LOYAL,
    INDIFFERENT,
    DISLOYAL
}

public class StudentStats : MonoBehaviour
{
    public string studentName = "Student";
    public Sprite profilePicture;
    public STUDENT_DESIRES studentDesire = STUDENT_DESIRES.NATURE;
    public List<STUDENT_TRAITS> studentTraits;
    public STUDENT_LOYALTY studentLoyalty = STUDENT_LOYALTY.INDIFFERENT;

    public void RandomiseStats()
    {
        studentTraits = new List<STUDENT_TRAITS>(Random.Range(1, 5));
        for (int j = 0; j < studentTraits.Capacity; j++)
        {
            studentTraits.Add((STUDENT_TRAITS)Random.Range(0, 4));
        }

        studentLoyalty = (STUDENT_LOYALTY)Random.Range(0, 3);
        studentDesire = (STUDENT_DESIRES)Random.Range(0, 2);
    }
}
