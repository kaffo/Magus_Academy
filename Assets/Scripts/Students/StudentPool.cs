using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentPool : MonoBehaviour
{
    public GameObject pooledStudentPrefab;
    public StudentInfoFrameManager studentInfoFrameManagerScript;

    private void Start()
    {
        if (pooledStudentPrefab == null || studentInfoFrameManagerScript == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        AddStudentsToPool(7);
    }

    public void AddStudentsToPool(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject studentObject = Instantiate(pooledStudentPrefab, transform);

            StudentStats studentStats = studentObject.GetComponent<StudentStats>();
            if (studentStats) { studentStats.RandomiseStats(); }
        }
        studentInfoFrameManagerScript.RefreshStudentInfoUI();
    }

    public int GetStudentPoolCount()
    {
        return transform.childCount;
    }
}
