using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RankingStats))]
public class PlayerUniversityStatsUpdate : MonoBehaviour
{
    private RankingStats myRankingStats
    {
        get
        {
            return gameObject.GetComponent<RankingStats>();
        }
    }

    private void Awake()
    {
        LecturerManager.Instance.OnHiredLecturerChange += EnrolledLecturerChangeHandler;
        StudentPool.Instance.OnEnrolledStudentChange += EnrolledStudentChangeHandler;
    }

    private void Start()
    {
        myRankingStats.lecturerNumber = LecturerManager.Instance.GetHiredLecturerCount();
        myRankingStats.studentNumber = StudentPool.Instance.GetEnrolledStudentCount();
    }

    private void EnrolledLecturerChangeHandler(int newEnrolled)
    {
        myRankingStats.lecturerNumber = newEnrolled;
    }

    private void EnrolledStudentChangeHandler(int newEnrolled)
    {
        myRankingStats.studentNumber = newEnrolled;
    }
}
