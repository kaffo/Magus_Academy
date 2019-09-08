using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MAGIC_SCHOOL
{
    NONE = -1,
    NATURE,
    CONJURATION
}

public class Classroom : Building
{
    public MAGIC_SCHOOL classroomType = MAGIC_SCHOOL.NATURE;
    public LecturerMovement myLecturer = null;

    public float GetSkillModifer()
    {
        // No lecturer, no teaching
        if (myLecturer == null || lecturersInside.Count <= 0) { return 0; }

        // How good is the lecturer at teaching
        if (myLecturer.myLecturerStats.lecturerSkills[classroomType] < 0.65)
        {
            return 1f;
        } else
        {
            return 1.25f;
        }
    }
}
