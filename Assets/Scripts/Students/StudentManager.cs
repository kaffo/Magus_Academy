using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : Singleton<StudentManager>
{
    public int currentStudentCapacity = 0;
    public int currentStudentNumber = 0; //TODO replace with list of students

    public GameObject studentUiPanel;
    public GameObject studentUiPrefab;

    private void Start()
    {
        if (studentUiPanel == null || studentUiPrefab == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        for (int i = 0; i < 30; i++)
        {
            Instantiate(studentUiPrefab, studentUiPanel.transform);
        }
    }
}
