using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LecturerCapacityTracker : MonoBehaviour
{
    public Text lecturerCapacityText;

    private void Awake()
    {
        LecturerManager.Instance.OnLecturerCapacityChange += LecturerCapacityChangeHandler;
        LecturerManager.Instance.OnHiredLecturerChange += EnrolledLecturerChangeHandler;
    }

    private void Start()
    {
        if (lecturerCapacityText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    private void LecturerCapacityChangeHandler(int newCapacity)
    {
        lecturerCapacityText.text = "Lecturers: " + LecturerManager.Instance.GetHiredLecturerCount() + "/" + newCapacity;
    }

    private void EnrolledLecturerChangeHandler(int newEnrolled)
    {
        lecturerCapacityText.text = "Lecturers: " + newEnrolled + "/" + LecturerManager.Instance.currentLecturerCapacity;
    }
}
