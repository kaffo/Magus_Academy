using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RankingStats))]
public class UniversityStatsRandomizer : MonoBehaviour
{
    private RankingStats myRankingStats
    {
        get
        {
            return gameObject.GetComponent<RankingStats>();
        }
    }

    private bool isUniImproving = false;
    private GameTime gameTime;

    private void Start()
    {
        gameTime = TimeManager.Instance.currentTime;
        isUniImproving = Random.value >= 0.50;

        gameTime.OnDayIncrement += OnDayChange;
        gameTime.OnMonthIncrement += OnMonthChange;

        UpdateUniversityStats();
    }

    private void UpdateUniversityStats()
    {
        if (isUniImproving)
        {
            // Chance of gaining 1 student
            myRankingStats.studentNumber += Random.Range(0, 2);
            // 5% chance to gain a lecturer
            myRankingStats.lecturerNumber += Random.value > 0.95 ? 1 : 0;
            myRankingStats.money += Random.Range(10, 100);
        } else
        {
            // Chance of losing 1 student
            myRankingStats.studentNumber -= Random.Range(0, 2);
            // 5% chance to lose a lecturer
            myRankingStats.lecturerNumber -= Random.value > 0.95 ? 1 : 0;
            myRankingStats.money -= Random.Range(10, 100);
        }

        // Don't let values fall below 0
        myRankingStats.studentNumber = myRankingStats.studentNumber < 0 ? 0 : myRankingStats.studentNumber;
        myRankingStats.lecturerNumber = myRankingStats.lecturerNumber < 0 ? 0 : myRankingStats.lecturerNumber;
        myRankingStats.money = myRankingStats.money < 0 ? 0 : myRankingStats.money;
    }

    private void OnDayChange()
    {
        UpdateUniversityStats();
    }

    private void OnMonthChange()
    {
        // Work out if the Uni is declining or not
        isUniImproving = Random.value >= 0.50;
    }
}
