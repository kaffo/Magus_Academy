using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int mixedCapacity = 10;
    public int studentCapacity = 0;
    public int lecturerCapacity = 0;

    protected HashSet<StudentMovement> studentsInside;
    protected HashSet<LecturerMovement> lecturersInside;
    protected GameTime gameTime;

    protected virtual void Awake()
    {
        studentsInside = new HashSet<StudentMovement>();
        lecturersInside = new HashSet<LecturerMovement>();
        gameTime = TimeManager.Instance.currentTime;
    }

    public bool StudentEnter(StudentMovement studentMoveScript)
    {
        if (StudentSpaceRemaining() > 0)
        {
            studentsInside.Add(studentMoveScript);
            studentMoveScript.HideStudent();
            return true;
        } else
        {
            Debug.Log("No space for student in " + gameObject.name);
            return false;
        }
    }

    public virtual void StudentExit(StudentMovement studentMoveScript)
    {
        if (studentsInside.Contains(studentMoveScript))
        {
            studentsInside.Remove(studentMoveScript);
            studentMoveScript.ShowStudent();
        }
        else
        {
            Debug.LogWarning("Student " + studentMoveScript.name + " tried to exit " + gameObject.name + " and failed");
        }
    }

    public bool LecturerEnter(LecturerMovement lecturerMoveScript)
    {
        if (LecturerSpaceRemaining() > 0)
        {
            lecturersInside.Add(lecturerMoveScript);
            lecturerMoveScript.HideLecturer();
            return true;
        }
        else
        {
            Debug.Log("No space for lecturer in " + gameObject.name);
            return false;
        }

    }

    public virtual void LecturerExit(LecturerMovement lecturerMoveScript)
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

    public int StudentSpaceRemaining()
    {
        return (mixedCapacity + studentCapacity) - studentsInside.Count;
    }

    public int LecturerSpaceRemaining()
    {
        return (mixedCapacity + lecturerCapacity) - lecturersInside.Count;
    }
}
