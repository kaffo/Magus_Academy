using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum BUILDINGS
{
    NONE = -1,
    SMALL_DORMS
};

[Serializable]
public class BuildingsScriptsDictionary : SerializableDictionary<BUILDINGS, BuildingSprites> { }

public class BuildingPlacement : Singleton<BuildingPlacement>
{
    [SerializeField]
    public BuildingsScriptsDictionary buildingsDict;

    [Header("References")]
    public Tilemap buildingsMap;
    public GameObject buildingInfoContentParent;
    public GameObject buildingInfoFramePrefab;

    private BUILDINGS selectedBuilding = BUILDINGS.NONE;

    private void Start()
    {
        if (buildingsMap == null || buildingInfoContentParent == null || buildingInfoFramePrefab == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        foreach (BUILDINGS possibleBuilding in buildingsDict.Keys)
        {
            GameObject buildingInfoFrame = Instantiate(buildingInfoFramePrefab, buildingInfoContentParent.transform);
            BuildingInfoFrameFiller fillerScript = buildingInfoFrame.GetComponent<BuildingInfoFrameFiller>();

            if (fillerScript)
            {
                fillerScript.myBuildingStatsScript = buildingsDict[possibleBuilding];
                fillerScript.myBuilding = possibleBuilding;
                fillerScript.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBuilding = BUILDINGS.SMALL_DORMS;
            buildingsDict[selectedBuilding].setGhostPreview(true);
        }

        if (Input.GetMouseButtonDown(1) && selectedBuilding != BUILDINGS.NONE)
        {
            buildingsDict[selectedBuilding].setGhostPreview(false);
            selectedBuilding = BUILDINGS.NONE;
        }

        if (Input.GetMouseButtonDown(0) && selectedBuilding != BUILDINGS.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3Int position = buildingsMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            buildingsDict[selectedBuilding].PlaceBuilding(buildingsMap, position.x, position.y, true);
        }
    }

    public void SetSelectedBuilding(BUILDINGS buildingToSelect)
    {
        if (buildingToSelect != BUILDINGS.NONE)
        {
            selectedBuilding = buildingToSelect;
            buildingsDict[selectedBuilding].setGhostPreview(true);
        } else if (selectedBuilding != BUILDINGS.NONE)
        {
            buildingsDict[selectedBuilding].setGhostPreview(false);
            selectedBuilding = BUILDINGS.NONE;
        }
    }
}
