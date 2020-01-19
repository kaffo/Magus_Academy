using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingStats : MonoBehaviour
{
    public string universityName = "University";
    [Range(1,100)]
    public int currentRanking = 1;
    public int studentNumber = 0;
    public int lecturerNumber = 0;
    public int money = 0;
    public int auditDeductions = 0;
}
