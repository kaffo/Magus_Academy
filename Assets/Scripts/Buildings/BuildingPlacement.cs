using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Tilemap buildingsMap;

    private BUILDINGS selectedBuilding = BUILDINGS.NONE;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBuilding = BUILDINGS.SMALL_DORMS;
            buildingsDict[selectedBuilding].setGhostPreview(true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            buildingsDict[selectedBuilding].setGhostPreview(false);
            selectedBuilding = BUILDINGS.NONE;
        }

        if (Input.GetMouseButtonDown(0) && selectedBuilding != BUILDINGS.NONE)
        {
            Vector3Int position = buildingsMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            buildingsDict[selectedBuilding].PlaceBuilding(buildingsMap, position.x, position.y, true);
        }
    }
}
