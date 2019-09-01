using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BuildingSprites))]
public class BuildOnStart : MonoBehaviour
{
    public Tilemap tileMapToDrawTo;
    public BuildingSprites myBuildingSprites;

    private void Start()
    {
        if (tileMapToDrawTo == null || myBuildingSprites == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myBuildingSprites.PlaceBuilding(tileMapToDrawTo, (int)transform.position.x, (int)transform.position.y, false);
    }
}
