using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BuildingSprites : MonoBehaviour
{
    public string name = "Default";

    [Header("References")]
    public Tilemap tileMapToDraw;
    public Tilemap ghostTilemap;

    [Header("Settings")]
    public int xSize = 1;
    public int ySize = 1;
    public List<TileBase> buildingTiles;

    [HideInInspector()]
    public bool ghostPreview = false;

    private bool previousGhostPreview = false;
    private Vector3Int previousPos = new Vector3Int();
    private void OnValidate()
    {
        if (buildingTiles.Count != xSize * ySize)
        {
            Debug.Log("Tile Count incorrect on: " + this.name + " on " + this.gameObject);
        }
    }

    private void Start()
    {
        if (tileMapToDraw == null || ghostTilemap == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        } 
    }

    public void PlaceBuilding(Tilemap gridToUse, int x, int y)
    {
        Vector3Int[] positionsArray = new Vector3Int[xSize * ySize];

        Vector3Int previousPosition = new Vector3Int(x, y, 0);
        int index = 0;
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if (gridToUse.HasTile(previousPosition))
                {
                    Debug.LogWarning("Tile already exists in coords: " + previousPosition);
                    return;
                }
                positionsArray[index++] = previousPosition;
                if (j + 1 < xSize) { previousPosition += new Vector3Int(1, 0, 0); }
                else { previousPosition.x = x; }
            }
            if (i + 1 < ySize) { previousPosition += new Vector3Int(0, -1, 0); }
        }

        gridToUse.SetTiles(positionsArray, buildingTiles.ToArray());
    }

    private void Update()
    {

        if (ghostPreview)
        {
            Vector3Int position = tileMapToDraw.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (previousPos != position)
            {
                ghostTilemap.ClearAllTiles();
                PlaceBuilding(ghostTilemap, position.x, position.y);
                previousPos = position;
            }
        }

        if (ghostPreview != previousGhostPreview)
        {
            previousGhostPreview = ghostPreview;
            if (!ghostPreview) { ghostTilemap.ClearAllTiles(); }
        }
    }
}
