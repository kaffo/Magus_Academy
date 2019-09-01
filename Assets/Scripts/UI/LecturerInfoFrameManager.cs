using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LecturerStatsLecturerInfoDictionary : SerializableDictionary<LecturerStats, LecturerInfoFrameFiller> { }

public class LecturerInfoFrameManager : MonoBehaviour
{
    public LecturerManager lecturerManagerScript;
    public GameObject lecturerInfoFramePrefab;
    public LecturerStatsLecturerInfoDictionary lecturerPoolTrackerDict;
    public bool usePooledLecturers = true;

    private void Start()
    {
        if (lecturerManagerScript == null || lecturerInfoFramePrefab == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    public void RefreshLecturerInfoUI()
    {
        GameObject poolToUse = usePooledLecturers ? lecturerManagerScript.selectionPoolGameObject : lecturerManagerScript.hiredPoolGameObject;
        List <LecturerStats> pooledLecturerStats = new List<LecturerStats>(poolToUse.transform.GetComponentsInChildren<LecturerStats>());

        bool removed = false;
        // Remove any pooled entries which have been deleted etc
        foreach (LecturerStats existingLecturer in lecturerPoolTrackerDict.Keys)
        {
            if (!pooledLecturerStats.Contains(existingLecturer))
            {
                Destroy(lecturerPoolTrackerDict[existingLecturer].gameObject);
                lecturerPoolTrackerDict.Remove(existingLecturer);
                removed = true;
                break;
            }
        }
        if (removed) { RefreshLecturerInfoUI(); }
        else
        {
            // Add any new entries which didn't exist before
            foreach (LecturerStats pooledLecturerStat in pooledLecturerStats)
            {
                if (!lecturerPoolTrackerDict.ContainsKey(pooledLecturerStat))
                {
                    GameObject lecturerFrameGameObject = Instantiate(lecturerInfoFramePrefab, transform);
                    LecturerInfoFrameFiller lecturerInfoScript = lecturerFrameGameObject.GetComponent<LecturerInfoFrameFiller>();

                    if (lecturerInfoScript)
                    {
                        lecturerPoolTrackerDict.Add(pooledLecturerStat, lecturerInfoScript);
                        lecturerInfoScript.myLecturerStatsReference = pooledLecturerStat;
                        lecturerInfoScript.enabled = true;
                    }
                }
            }
        }
    }
}
