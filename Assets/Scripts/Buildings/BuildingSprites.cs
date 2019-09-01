using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BuildingSprites : MonoBehaviour
{
    [Header("Building Stats")]
    public string buildingName = "Default";
    public int price = 1000;
    [TextArea]
    public string description = "Empty Description";
    public Sprite buildingImage;

    [Header("References")]
    public Tilemap ghostTilemap;
    public GameObject prefabToSpawn;

    [Header("Settings")]
    public int xSize = 1;
    public int ySize = 1;
    public bool useOffset = false;
    public int xOffset = 0;
    public int yOffset = 0;
    public List<TileBase> buildingTiles;


    private bool ghostPreview = false;
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
        if (ghostTilemap == null || prefabToSpawn == null)
            Debug.Log(this.name + " on " + this.gameObject + " may be missing an item");
    }

    public void PlaceBuilding(Tilemap gridToUse, int x, int y, bool createBuildingPrefab = false)
    {
        Vector3Int[] positionsArray = new Vector3Int[xSize * ySize];

        if (useOffset) { x += xOffset; y += yOffset; }

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
            if (i + 1 < ySize) { previousPosition += new Vector3Int(0, 1, 0); }
        }

        gridToUse.SetTiles(positionsArray, buildingTiles.ToArray());

        if (createBuildingPrefab)
            Instantiate(prefabToSpawn, new Vector3(x, y, 0), new Quaternion(), transform);
    }

    public void setGhostPreview(bool newGhostPreview)
    {
        ghostPreview = newGhostPreview;
        if (!ghostPreview) { ghostTilemap.ClearAllTiles(); }
    }

    private void Update()
    {
        if (ghostPreview)
        {
            Vector3Int position = ghostTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (previousPos != position)
            {
                ghostTilemap.ClearAllTiles();
                PlaceBuilding(ghostTilemap, position.x, position.y);
                previousPos = position;
            }
        }
    }
}
