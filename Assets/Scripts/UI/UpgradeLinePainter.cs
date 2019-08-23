using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLinePainter : MonoBehaviour
{
    public GameObject linePrefabGameObject;
    public float uiScale = 20f;

    private void Start()
    {
        if (!linePrefabGameObject)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        UpgradeDependancy[] childDependancyList = transform.GetComponentsInChildren<UpgradeDependancy>(true);

        foreach (UpgradeDependancy parentUpgradeDependancy in childDependancyList)
        {
            foreach (UpgradeDependancy childUpgradeDependancy in parentUpgradeDependancy.myUpgradeDependancies)
            {
                Debug.Log("Parent " + parentUpgradeDependancy.gameObject.name + " pos: " + parentUpgradeDependancy.transform.position);
                Debug.Log("Child " + childUpgradeDependancy.gameObject.name + " pos: " + childUpgradeDependancy.transform.position + '\n');

                GameObject lineGameobject = Instantiate(linePrefabGameObject, transform);

                Vector3[] worldCorners = new Vector3[4];
                parentUpgradeDependancy.GetComponent<RectTransform>().GetWorldCorners(worldCorners);
                Vector3 parentPosition = (worldCorners[0] + worldCorners[2]) / 2;

                childUpgradeDependancy.GetComponent<RectTransform>().GetWorldCorners(worldCorners);
                Vector3 childPosition = (worldCorners[0] + worldCorners[2]) / 2;
                RectTransform lineRect = lineGameobject.GetComponent<RectTransform>();

                Vector3 midpoint = (childPosition + parentPosition) / 2;
                float pointDistance = (parentPosition - childPosition).magnitude;

                float angle = Mathf.Atan2(childPosition.x - parentPosition.x, parentPosition.y - childPosition.y);
                if (angle < 0.0) { angle += Mathf.PI * 2; }
                angle *= Mathf.Rad2Deg;

                lineRect.rotation = Quaternion.Euler(0, 0, angle);
                lineRect.position = midpoint;
                lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, pointDistance * uiScale);
            }
        }

    }
}
