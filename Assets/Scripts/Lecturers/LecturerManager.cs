using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System.Reflection;

public class LecturerManager : Singleton<LecturerManager>
{
    public int currentLecturerCapacity
    {
        get { return m_currentLecturerCapacity; }
        set
        {
            if (m_currentLecturerCapacity == value) return;
            m_currentLecturerCapacity = value;
            if (OnLecturerCapacityChange != null)
                OnLecturerCapacityChange(m_currentLecturerCapacity);
        }
    }
    public delegate void OnLecturerCapacityChangeDelegate(int newCapacity);
    public event OnLecturerCapacityChangeDelegate OnLecturerCapacityChange;

    public delegate void OnHiredLecturerChangeDelegate(int newHired);
    public event OnHiredLecturerChangeDelegate OnHiredLecturerChange;

    [Header("References")]
    public GameObject pooledLecturerPrefab;
    public GameObject hiredLecturerPrefab;
    public List<LecturerInfoFrameManager> lecturerInfoFrameManagerScripts;
    public PolyNav2D navMesh;

    [Header("Internal References")]
    public GameObject selectionPoolGameObject;
    public GameObject hiredPoolGameObject;

    private int m_currentLecturerCapacity = 3;


    private void Start()
    {
        if (pooledLecturerPrefab == null || hiredLecturerPrefab == null || navMesh == null ||
            selectionPoolGameObject == null || hiredPoolGameObject == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        AddLecturersToPool(5);
        OnLecturerCapacityChange(m_currentLecturerCapacity);
    }

    public void AddLecturersToPool(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject lecturerObject = Instantiate(pooledLecturerPrefab, selectionPoolGameObject.transform);

            LecturerStats lecturerStats = lecturerObject.GetComponent<LecturerStats>();
            if (lecturerStats) { lecturerStats.RandomiseStats(); }
        }

        foreach (LecturerInfoFrameManager lecturerInfoManager in lecturerInfoFrameManagerScripts)
        {
            lecturerInfoManager.RefreshLecturerInfoUI();
        }
    }

    public void HireLecturer(LecturerStats lecturerToHire)
    {
        GameObject hiredLecturers = Instantiate(hiredLecturerPrefab, hiredPoolGameObject.transform);
        LecturerStats enrolledStats = hiredLecturers.GetComponent<LecturerStats>();
        PolyNavAgent polyNavAgent = hiredLecturers.GetComponent<PolyNavAgent>();


        if (enrolledStats)
        {
            CopyClassValues(lecturerToHire, enrolledStats);
        }

        if (polyNavAgent)
        {
            polyNavAgent.map = navMesh;
            polyNavAgent.enabled = true;
        }

        Destroy(lecturerToHire.gameObject);
        StartCoroutine(RefreshAfterUpdate());
        OnHiredLecturerChange(GetHiredLecturerCount());
    }

    private void CopyClassValues(LecturerStats sourceComp, LecturerStats targetComp)
    {
        FieldInfo[] sourceFields = sourceComp.GetType().GetFields(BindingFlags.Public |
                                                         BindingFlags.NonPublic |
                                                         BindingFlags.Instance);
        int i = 0;
        for (i = 0; i < sourceFields.Length; i++)
        {
            var value = sourceFields[i].GetValue(sourceComp);
            sourceFields[i].SetValue(targetComp, value);
        }
    }

    private IEnumerator RefreshAfterUpdate()
    {
        yield return new WaitForFixedUpdate();
        foreach (LecturerInfoFrameManager lecturerInfoManager in lecturerInfoFrameManagerScripts)
        {
            lecturerInfoManager.RefreshLecturerInfoUI();
        }
    }

    public int GetLecturerPoolCount()
    {
        return selectionPoolGameObject.transform.childCount;
    }

    public int GetHiredLecturerCount()
    {
        return hiredPoolGameObject.transform.childCount;
    }
}
