using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MAGIC_SCHOOL
{
    NONE = -1,
    NATURE,
    ELEMENTAL
}

public class Classroom : Building
{
    public MAGIC_SCHOOL classroomType = MAGIC_SCHOOL.NATURE;
    public LecturerMovement myLecturer = null;
}
