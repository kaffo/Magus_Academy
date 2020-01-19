using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingsManager : Singleton<RankingsManager>
{
    private static readonly List<string> UniversityNames = new List<string>{
        "Grand Mountain School of Fine Arts",
        "Winterville College",
        "Sunshine Elementary",
        "Sun Valley Institute",
        "Paramount Elementary",
        "Bear River School for Boys",
        "Grandview University",
        "Storm Coast High",
        "Riverbank Institute",
        "Harbor View Grammar School",
        "Green Valley School for Girls",
        "Angelwood Institute",
        "Grand Ridge School for Boys",
        "East Shores Charter School",
        "Great Oak High School",
        "Storm Coast Charter School",
        "Pleasant Valley Technical School",
        "Trinity School of Fine Arts",
        "Summit College",
        "Pleasant Grove Institute",
        "Lone Oak Middle School",
        "Grand Mountain High School",
        "Oak Hills Conservatory",
        "Riverview Grammar School",
        "Independence Charter School",
        "Panorama University",
        "Grand Mountain School of Fine Arts",
        "Lakewood Grammar School",
        "Laguna Bay School of Fine Arts",
        "Big Valley School of Fine Arts",
        "Faraday School of Fine Arts",
        "West Shores Institute",
        "White Mountain School for Girls",
        "Green Meadows Middle School",
        "Independence Academy",
        "Winters Kindergarten",
        "Waterford Grammar School",
        "River Valley Technical School",
        "Ravenwood Technical School",
        "Laguna Beach Kindergarten",
        "Garden Grove Charter School",
        "Sunset Grammar School",
        "Little Valley Charter School",
        "Ridgeview School for Boys",
        "Little Valley School for Girls",
        "Enterprise University",
        "Hillview High",
        "Saint Mary's High School",
        "Saint Helena Middle School",
        "Sandalwood Kindergarten",
        "Pacific Grove College",
        "Redlands Kindergarten",
        "Pleasant Valley Kindergarten",
        "Seaside Charter School",
        "Pleasant Hill Charter School",
        "Apple Valley High School",
        "Hawking Grammar School",
        "Grand Mountain Academy",
        "Maple Leaf Secondary School",
        "Rutherford School for Boys",
        "Greenlands Grammar School",
        "Grand Ridge Elementary",
        "Sandalwood Charter School",
        "Monarch Conservatory",
        "Monarch University",
        "Woodcreek Elementary",
        "Silver Oak School for Boys",
        "Copper Cove School of Fine Arts",
        "Skyline Grammar School",
        "Pleasant Grove University",
        "Paramount Charter School",
        "Pleasant Grove College",
        "Panorama Academy",
        "Oceanside Middle School",
        "Pacific Grove Technical School",
        "Woodcreek University",
        "Mountainview Elementary",
        "Elk Grove Elementary",
        "Seacoast Grammar School",
        "Promise Academy",
        "Maple Leaf Middle School",
        "Bear Valley Technical School",
        "Summit Secondary School",
        "East Bridge School for Girls",
        "Marble Hills Technical School",
        "Pleasant Valley School of Fine Arts",
        "Crossroads Grammar School",
        "Summerville Grammar School",
        "Lincoln College",
        "Greenlands Conservatory",
        "Meadows Secondary School",
        "Green Valley High School",
        "River Fork College",
        "Little Valley Charter School",
        "Cypress University",
        "Trinity School",
        "Laguna Creek Academy",
        "Rainbow Institute",
        "Meadows Kindergarten",
        "River Valley Secondary School"
    };

    [Header("References")]
    public GameObject universityRankingPrefab;

    [Header("Internal References")]
    public GameObject rankingContainer;

    private GameTime gameTime;
    private List<RankingStats> rankingStatList;

    private void Start()
    {
        if (universityRankingPrefab == null || rankingContainer == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        gameTime = TimeManager.Instance.currentTime;
        gameTime.OnYearIncrement += OnYearChange;
        gameTime.OnDayIncrement += OnFirstDay;
        rankingStatList = new List<RankingStats>();

        PopulateRankings();
    }

    private void PopulateRankings()
    {
        for (int i = 0; i < 99; i++)
        {
            GameObject currentRanking = Instantiate(universityRankingPrefab, rankingContainer.transform);

            RankingStats currentStats = currentRanking.GetComponent<RankingStats>();
            if (currentStats != null)
            {
                currentStats.universityName = UniversityNames[i];
                currentStats.currentRanking = 0; // Unranked until end of day 1
                currentStats.lecturerNumber = 1;
                // Add randomizer to non-player Universities
                currentRanking.AddComponent<UniversityStatsRandomizer>();
                rankingStatList.Add(currentStats);
            }
        }
        GameObject playerRanking = Instantiate(universityRankingPrefab, rankingContainer.transform);

        RankingStats playerStats = playerRanking.GetComponent<RankingStats>();
        if (playerStats != null)
        {
            playerStats.universityName = "Player University";
            playerStats.currentRanking = 0; // Unranked until end of day 1
            playerRanking.AddComponent<PlayerUniversityStatsUpdate>();
            rankingStatList.Add(playerStats);
        }
    }

    public void UpdateRankings()
    {
        Dictionary<int, RankingStats> rankDict = new Dictionary<int, RankingStats>();

        foreach (RankingStats currentRanking in rankingStatList)
        {
            // A terrible caclulation which needs changed
            int score = (currentRanking.studentNumber * 10) + (currentRanking.lecturerNumber * 1000) + currentRanking.money - (int)currentRanking.auditDeductions;
            while (rankDict.ContainsKey(score))
            {
                score++;
            }
            rankDict.Add(score, currentRanking);
        }

        // Get the cacluated scores and sort them
        List<int> scoreList = new List<int>(rankDict.Keys);
        scoreList.Sort((a, b) => b.CompareTo(a));

        for (int i = 0; i < scoreList.Count; i++)
        {
            rankDict[scoreList[i]].currentRanking = i + 1;
        }
    }

    private void OnFirstDay()
    {
        // Update rankings at end of Day 1 then remove the event listener
        UpdateRankings();
        gameTime.OnDayIncrement -= OnFirstDay;
    }

    private void OnYearChange()
    {
        UpdateRankings();
    }
}