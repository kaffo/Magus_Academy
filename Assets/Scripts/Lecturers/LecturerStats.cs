using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

[Serializable]
public class MagicTypeFloatDictionary : SerializableDictionary<MAGIC_SCHOOL, float> { }

public class LecturerStats : MonoBehaviour
{
    public string lecturerName = "Lecturer";
    public Sprite profilePicture;
    public LECTURER_DESIRES lecturerDesire = LECTURER_DESIRES.TEACHING;
    public MagicTypeFloatDictionary lecturerSkills;
    public List<LECTURER_TRAITS> lecturerTraits;
    public LECTURER_LOYALTY lecturerLoyalty = LECTURER_LOYALTY.INDIFFERENT;
    public bool isHired = false;

    public void RandomiseStats()
    {
        lecturerTraits = new List<LECTURER_TRAITS>(UnityEngine.Random.Range(1, 5));

        lecturerSkills = new MagicTypeFloatDictionary();
        var magicTypes = Enum.GetValues(typeof(MAGIC_SCHOOL)).Cast<MAGIC_SCHOOL>();
        foreach (MAGIC_SCHOOL currentMagicSchool in magicTypes)
        {
            if (currentMagicSchool == MAGIC_SCHOOL.NONE) { continue; }
            lecturerSkills.Add(currentMagicSchool, (float)System.Math.Round(UnityEngine.Random.Range(0f, 1f), 2));
        }

        for (int j = 0; j < lecturerTraits.Capacity; j++)
        {
            lecturerTraits.Add((LECTURER_TRAITS)UnityEngine.Random.Range(0, 4));
        }

        lecturerLoyalty = (LECTURER_LOYALTY)UnityEngine.Random.Range(0, 3);
        lecturerDesire = (LECTURER_DESIRES)UnityEngine.Random.Range(0, 2);
    }
}
