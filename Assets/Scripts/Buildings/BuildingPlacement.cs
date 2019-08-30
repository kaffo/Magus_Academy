using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum BUILDINGS
{
    NONE = -1,
    SMALL_DORMS,
    NATURE_CLASSROOM
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

    [Header("Internal References")]
    public GameObject domitoryPoolObject;
    public GameObject classroomPoolObject;

    private BUILDINGS selectedBuilding = BUILDINGS.NONE;

    private void Start()
    {
        if (buildingsMap == null || buildingInfoContentParent == null || buildingInfoFramePrefab == null || domitoryPoolObject == null ||
            classroomPoolObject == null)
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
            if (selectedBuilding != BUILDINGS.NONE) { buildingsDict[selectedBuilding].setGhostPreview(false); }
            selectedBuilding = BUILDINGS.SMALL_DORMS;
            buildingsDict[selectedBuilding].setGhostPreview(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (selectedBuilding != BUILDINGS.NONE) { buildingsDict[selectedBuilding].setGhostPreview(false); }
            selectedBuilding = BUILDINGS.NATURE_CLASSROOM;
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
            if (selectedBuilding != BUILDINGS.NONE) { buildingsDict[selectedBuilding].setGhostPreview(false); }
            selectedBuilding = buildingToSelect;
            buildingsDict[selectedBuilding].setGhostPreview(true);
        } else if (selectedBuilding != BUILDINGS.NONE)
        {
            buildingsDict[selectedBuilding].setGhostPreview(false);
            selectedBuilding = BUILDINGS.NONE;
        }
    }

    public Classroom FindClassroom(MAGIC_SCHOOL classroomTypeToFind)
    {
        foreach (Classroom classroomToCheck in transform.GetComponentsInChildren<Classroom>())
        {
            if (classroomToCheck.classroomType == classroomTypeToFind) { return classroomToCheck; }
        }
        return null;
    }
}
