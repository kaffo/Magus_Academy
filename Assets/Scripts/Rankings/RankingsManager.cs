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

    private void Start()
    {
        if (universityRankingPrefab == null || rankingContainer == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        gameTime = TimeManager.Instance.currentTime;

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
                currentStats.UniversityName = UniversityNames[i];
                currentStats.currentRanking = i + 1;
            }
        }
        GameObject playerRanking = Instantiate(universityRankingPrefab, rankingContainer.transform);

        RankingStats playerStats = playerRanking.GetComponent<RankingStats>();
        if (playerStats != null)
        {
            playerStats.UniversityName = "Player University";
            playerStats.currentRanking = 100;
        }
    }
}