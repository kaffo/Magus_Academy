using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentCapacityTracker : MonoBehaviour
{
    public Text studentCapacityText;

    private void Start()
    {
        if (studentCapacityText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
        StudentPool.Instance.OnStudentCapacityChange += StudentCapacityChangeHandler;
        StudentPool.Instance.OnEnrolledStudentChange += EnrolledStudentChangeHandler;
    }

    private void StudentCapacityChangeHandler(int newCapacity)
    {
        studentCapacityText.text = "Students: " + StudentPool.Instance.GetEnrolledStudentCount() + "/" + newCapacity; 
    }

    private void EnrolledStudentChangeHandler(int newEnrolled)
    {
        studentCapacityText.text = "Students: " + newEnrolled + "/" + StudentPool.Instance.currentStudentCapacity;
    }
}
