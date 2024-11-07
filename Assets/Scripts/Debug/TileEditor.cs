// Scripts/Debug/TileEditor.cs
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TileEditor : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;

    public TileType selectedTileType;  // Set through the custom editor
    public int radius = 0;             // Set through the custom editor

    public HexCell selectedCell;
    private HexGrid hexGrid;
    private HexMapVisuals hexMapVisuals;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        hexGrid = FindObjectOfType<HexGrid>();
        hexMapVisuals = FindObjectOfType<HexMapVisuals>();

        if (hexGrid == null || hexMapVisuals == null)
        {
            Debug.LogError("HexGrid or HexMapVisuals not found in the scene.");
        }
    }

    // Selects the tile when clicked in the editor
    public void SelectTile()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            selectedCell = hit.transform.GetComponentInParent<HexCell>();
            Debug.Log(selectedCell != null 
                ? $"Tile selected at {selectedCell.OffsetCoordinates}." 
                : "No tile selected.");
        }
    }

    // Applies the selected tile type to the chosen cell and surrounding cells
    public void ApplyTileType()
    {
        if (selectedCell == null)
        {
            Debug.LogWarning("No tile selected. Click on a tile to select it first.");
            return;
        }

        List<HexCell> cellsToChange = hexGrid.GetHexesInRange(selectedCell, radius);

        foreach (HexCell cell in cellsToChange)
        {
            UpdateTileVisuals(cell, selectedTileType);
        }
    }

    // Sets fog of war on all tiles to active (cover all with fog)
    public void ShowFogOfWar()
    {
        foreach (var cell in hexGrid.cells.Values)
        {
            Transform fogOfWar = cell.VisualRepresentation.transform.Find("FogOfWar");
            if (fogOfWar != null)
            {
                fogOfWar.gameObject.SetActive(true);
            }
        }
        Debug.Log("Fog of war enabled on all tiles.");
    }

    // Sets fog of war on all tiles to inactive (reveal all)
    public void HideFogOfWar()
    {
        foreach (var cell in hexGrid.cells.Values)
        {
            Transform fogOfWar = cell.VisualRepresentation.transform.Find("FogOfWar");
            if (fogOfWar != null)
            {
                fogOfWar.gameObject.SetActive(false);
            }
        }
        Debug.Log("Fog of war disabled on all tiles.");
    }

    // Helper method to update tile visuals
    private void UpdateTileVisuals(HexCell cell, TileType newTileType)
    {
        if (!hexMapVisuals.tileDataDictionary.ContainsKey(newTileType))
        {
            Debug.LogError("TileData not found for the selected tile type.");
            return;
        }

        HexTileData tileData = hexMapVisuals.tileDataDictionary[newTileType];
        cell.TerrainType = tileData;

        if (cell.VisualRepresentation != null)
        {
            DestroyImmediate(cell.VisualRepresentation); // Ensure immediate destruction in the editor
        }

        GameObject newTile = TileFactory.CreateTile(tileData, cell.transform.position, cell.transform);
        if (newTile != null)
        {
            newTile.transform.localPosition = Vector3.zero;
            cell.VisualRepresentation = newTile;
        }
    }
}
