using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StudentStatsStudentInfoDictionary : SerializableDictionary<StudentStats, StudentInfoFrameFiller> { }

public class StudentInfoFrameManager : MonoBehaviour
{
    public StudentPool studentPoolScript;
    public GameObject studentInfoFramePrefab;
    public StudentStatsStudentInfoDictionary studentPoolTrackerDict;

    private void Start()
    {
        if (studentPoolScript == null || studentInfoFramePrefab == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    public void RefreshStudentInfoUI()
    {
        List<StudentStats> pooledStudentStats = new List<StudentStats>(studentPoolScript.selectionPoolGameObject.transform.GetComponentsInChildren<StudentStats>());

        bool removed = false;
        // Remove any pooled entries which have been deleted etc
        foreach (StudentStats existingStudent in studentPoolTrackerDict.Keys)
        {
            if (!pooledStudentStats.Contains(existingStudent)) {
                Destroy(studentPoolTrackerDict[existingStudent].gameObject);
                studentPoolTrackerDict.Remove(existingStudent);
                removed = true;
                break;
            }
        }
        if (removed) { RefreshStudentInfoUI(); }
        else
        {
            // Add any new entries which didn't exist before
            foreach (StudentStats pooledStudentStat in pooledStudentStats)
            {
                if (!studentPoolTrackerDict.ContainsKey(pooledStudentStat))
                {
                    GameObject studentFrameGameObject = Instantiate(studentInfoFramePrefab, transform);
                    StudentInfoFrameFiller studentInfoScript = studentFrameGameObject.GetComponent<StudentInfoFrameFiller>();

                    if (studentInfoScript)
                    {
                        studentPoolTrackerDict.Add(pooledStudentStat, studentInfoScript);
                        studentInfoScript.myStudentStatsReference = pooledStudentStat;
                        studentInfoScript.enabled = true;
                    }
                }
            }
        }
    }
}
