using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectButton : MonoBehaviour
{
    public BUILDINGS buildingToSelect = BUILDINGS.NONE;
    private Button myButton;

    private void Start()
    {
        myButton = this.GetComponent<Button>();
        if (myButton == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
        myButton.onClick.AddListener(MyButtonOnClick);
    }

    private void MyButtonOnClick()
    {
        BuildingPlacement.Instance.SetSelectedBuilding(buildingToSelect);
    }
}
