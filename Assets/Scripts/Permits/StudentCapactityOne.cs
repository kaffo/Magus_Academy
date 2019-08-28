using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentCapactityOne : MonoBehaviour
{
    public int capacityIncrease = 5;

    private void Start()
    {
        StudentPool.Instance.currentStudentCapacity += 5;
    }
}
