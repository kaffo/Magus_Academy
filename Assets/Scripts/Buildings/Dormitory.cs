using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitory : MonoBehaviour
{
    public int capacity = 10;

    public List<StudentStats> boardingStudents;
    private List<GameObject> studentsInside;
    private GameTime gameTime;

    private void Awake()
    {
        studentsInside = new List<GameObject>(capacity);
        gameTime = TimeManager.Instance.currentTime;
    }

    private void OnEnable()
    {
        gameTime.OnHourIncrement += OnHourChange;
    }

    private void OnDisable()
    {
        gameTime.OnHourIncrement += OnHourChange;
    }

    public void StudentEnter(GameObject studentGameObject)
    {
        studentsInside.Add(studentGameObject);
        studentGameObject.SetActive(false);
    }

    public void StudentExit(GameObject studentGameObject)
    {
        if (studentsInside.Contains(studentGameObject))
        {
            studentsInside.Remove(studentGameObject);
            studentGameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Student " + studentGameObject.name + " tried to exit " + gameObject.name + " and failed");
        }
    }

    private void OnHourChange()
    {
        if (gameTime.hours == 7)
        {
            List<GameObject> studentsToRemove = new List<GameObject>(studentsInside);
            foreach (GameObject studentGameObject in studentsToRemove)
            {
                StudentExit(studentGameObject);
            }
        } 
    }

    public int AccommodationSpaceRemaining()
    {
        return capacity - boardingStudents.Count;
    }
}
