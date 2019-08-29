using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoFrameFiller : MonoBehaviour
{
    [Header("External References")]
    public BuildingSprites myBuildingStatsScript;
    public BUILDINGS myBuilding = BUILDINGS.NONE;

    [Header("Internal References")]
    public Image buildingImage;
    public Text buildingNameText;
    public Text buildingPriceText;
    public Text buildingDescriptionText;
    public BuildingSelectButton buildingSelectButton;

    private void Start()
    {
        if (myBuildingStatsScript == null || buildingImage == null || buildingNameText == null || buildingDescriptionText == null || buildingSelectButton == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        buildingImage.sprite = myBuildingStatsScript.buildingImage;
        buildingNameText.text = myBuildingStatsScript.buildingName;
        buildingPriceText.text = "$" + myBuildingStatsScript.price.ToString();
        buildingDescriptionText.text = myBuildingStatsScript.description;
        buildingSelectButton.buildingToSelect = myBuilding;
    }
}
