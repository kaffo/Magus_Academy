﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MageAcademy;

public enum STUDENT_TRAITS
{
    LAZY,
    HAPPY,
    SAD,
    HARDWORKING
}

public class StudentStats : MonoBehaviour
{
    public string studentName = "Student";
    public Sprite profilePicture;
    public MAGIC_SCHOOL studentDesire = MAGIC_SCHOOL.NATURE;
    public MagicTypeFloatDictionary studentSkills;
    public List<STUDENT_TRAITS> studentTraits;
    public float studyPeriod = 0.5f;

    private float lastStudy = 0f;

    public void RandomiseStats()
    {
        studentSkills = new MagicTypeFloatDictionary();
        var magicTypes = Enum.GetValues(typeof(MAGIC_SCHOOL)).Cast<MAGIC_SCHOOL>();
        foreach (MAGIC_SCHOOL currentMagicSchool in magicTypes)
        {
            if (currentMagicSchool == MAGIC_SCHOOL.NONE) { continue; }
            studentSkills.Add(currentMagicSchool, (float)System.Math.Round(UnityEngine.Random.Range(0f, 0.4f), 2));
        }

        studentTraits = new List<STUDENT_TRAITS>(UnityEngine.Random.Range(1, 5));
        for (int j = 0; j < studentTraits.Capacity; j++)
        {
            studentTraits.Add((STUDENT_TRAITS)UnityEngine.Random.Range(0, 4));
        }

        studentDesire = (MAGIC_SCHOOL)UnityEngine.Random.Range(0, 2);
    }

    public void StudyMagic(MAGIC_SCHOOL schoolToStudy, float studyModifer)
    {
        // Only increment student skill if study cooldown has passed
        if (lastStudy + studyPeriod < Time.time)
        {
            studentSkills[schoolToStudy] += Constants.STUDENT_LEARN_RATE * studyModifer;
            lastStudy = Time.time;
        }
    }
}
