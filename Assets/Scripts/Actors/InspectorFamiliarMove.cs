using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorFamiliarMove : MonoBehaviour
{
    public Vector3 startCoords = new Vector3(40, 40, 0);
    public Vector3 endCoords = new Vector3(-40, -40, 0);
    public float moveSpeed = 0.01f;

    void Start()
    {
        transform.position = startCoords;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endCoords, moveSpeed);
    }
}
