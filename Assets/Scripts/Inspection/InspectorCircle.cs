using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorCircle : MonoBehaviour
{
    public float inspectorRadius = 3f;
    public GameObject visibleDisk;

    private List<GameObject> seenStudents;
    private List<GameObject> seenBuildings;

    private void Start()
    {
        if (visibleDisk != null)
        {
            Vector3 scale = new Vector3(inspectorRadius, 0.1f, inspectorRadius);
            visibleDisk.transform.localScale = scale;
        }

        seenStudents = new List<GameObject>();
        seenBuildings = new List<GameObject>();
    }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), inspectorRadius);
        foreach (Collider2D collider in colliders)
        {
            GameObject currentGameObject = collider.gameObject;
            StudentMovement studentMoveScript = currentGameObject.GetComponent<StudentMovement>();
            Building buildingScript = currentGameObject.GetComponent<Building>();

            if (!seenStudents.Contains(currentGameObject) && studentMoveScript != null)
            {
                Debug.Log($"Inspector noted {currentGameObject.GetComponent<StudentStats>().studentName}");
                seenStudents.Add(currentGameObject);
                break;
            }

            if (!seenBuildings.Contains(currentGameObject) && buildingScript != null)
            {
                Debug.Log($"Inspector noted {buildingScript.name}");
                seenBuildings.Add(currentGameObject);
                break;
            }
        }
    }
}
