// Scripts/Debug/TileEditor.cs
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TileEditor : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;

    public TileType selectedTileType;  // Set through the custom editor
    public int radius = 0;             // Tile application radius

    public HexCell selectedCell;
    private HexGrid hexGrid;
    private HexMapVisuals hexMapVisuals;

    void Start()
    {
        mainCamera = mainCamera ? mainCamera : Camera.main;
        hexGrid = FindObjectOfType<HexGrid>();
        hexMapVisuals = FindObjectOfType<HexMapVisuals>();

        if (!hexGrid || !hexMapVisuals)
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

    // Enable fog of war on all tiles
    public void ShowFogOfWar()
    {
        foreach (var cell in hexGrid.cells.Values)
        {
            var fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog) fog.gameObject.SetActive(true);
        }
    }

    // Disable fog of war on all tiles
    public void HideFogOfWar()
    {
        foreach (var cell in hexGrid.cells.Values)
        {
            var fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog) fog.gameObject.SetActive(false);
        }
    }

    // Updates the visuals for the specified tile based on the selected type
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
            DestroyImmediate(cell.VisualRepresentation);
        }

        GameObject newTile = TileFactory.CreateTile(tileData, cell.transform.position, cell.transform);
        if (newTile != null)
        {
            newTile.transform.localPosition = Vector3.zero;
            cell.VisualRepresentation = newTile;
        }
    }
}
