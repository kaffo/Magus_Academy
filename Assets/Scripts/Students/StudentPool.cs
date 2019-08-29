using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using PolyNav;

public class StudentPool : Singleton<StudentPool>
{
    public int currentStudentCapacity
    {
        get { return m_currentStudentCapacity; }
        set
        {
            if (m_currentStudentCapacity == value) return;
            m_currentStudentCapacity = value;
            if (OnStudentCapacityChange != null)
                OnStudentCapacityChange(m_currentStudentCapacity);
        }
    }
    public delegate void OnStudentCapacityChangeDelegate(int newCapacity);
    public event OnStudentCapacityChangeDelegate OnStudentCapacityChange;

    public delegate void OnEnrolledStudentChangeDelegate(int newEnrolled);
    public event OnEnrolledStudentChangeDelegate OnEnrolledStudentChange;

    [Header("References")]
    public GameObject pooledStudentPrefab;
    public GameObject enrolledStudentPrefab;
    public StudentInfoFrameManager studentInfoFrameManagerScript;
    public PolyNav2D navMesh;

    [Header("Internal References")]
    public GameObject selectionPoolGameObject;
    public GameObject enrolledGameObject;

    private int m_currentStudentCapacity = 0;
    

    private void Start()
    {
        if (pooledStudentPrefab == null || enrolledStudentPrefab  == null || studentInfoFrameManagerScript == null || navMesh == null ||
            selectionPoolGameObject == null || enrolledGameObject == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        AddStudentsToPool(7);
        OnStudentCapacityChange(m_currentStudentCapacity);
    }

    public void AddStudentsToPool(int numToAdd)
    {
        for (int i = 0; i < numToAdd; i++)
        {
            GameObject studentObject = Instantiate(pooledStudentPrefab, selectionPoolGameObject.transform);

            StudentStats studentStats = studentObject.GetComponent<StudentStats>();
            if (studentStats) { studentStats.RandomiseStats(); }
        }
        studentInfoFrameManagerScript.RefreshStudentInfoUI();
    }

    public void EnrollStudent(StudentStats studentToEnroll)
    {
        GameObject enrolledStudent = Instantiate(enrolledStudentPrefab, enrolledGameObject.transform);
        StudentStats enrolledStats = enrolledStudent.GetComponent<StudentStats>();
        PolyNavAgent polyNavAgent = enrolledStudent.GetComponent<PolyNavAgent>();


        if (enrolledStats)
        {
            CopyClassValues(studentToEnroll, enrolledStats);
        }

        if (polyNavAgent)
        {
            polyNavAgent.map = navMesh;
            polyNavAgent.enabled = true;
        }

        Destroy(studentToEnroll.gameObject);
        StartCoroutine(RefreshAfterUpdate());
        OnEnrolledStudentChange(GetEnrolledStudentCount());
    }

    private void CopyClassValues(StudentStats sourceComp, StudentStats targetComp)
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
        studentInfoFrameManagerScript.RefreshStudentInfoUI();
    }

    public int GetStudentPoolCount()
    {
        return selectionPoolGameObject.transform.childCount;
    }

    public int GetEnrolledStudentCount()
    {
        return enrolledGameObject.transform.childCount;
    }
}
