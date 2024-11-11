// Scripts/Debug/TileEditor.cs
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TileEditor : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;

    public int selectedTerrainID;  // Terrain ID set through the custom editor
    public int radius = 0;         // Tile application radius

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

    // Applies the selected terrain ID to the chosen cell and surrounding cells
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
            UpdateTileVisuals(cell, selectedTerrainID);
        }
    }

    private void UpdateTileVisuals(HexCell cell, int terrainID)
    {
        if (!hexMapVisuals.tileDataDictionary.ContainsKey(terrainID))
        {
            Debug.LogError("TileData not found for the selected terrain ID.");
            return;
        }

        HexTileData tileData = hexMapVisuals.tileDataDictionary[terrainID];
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
    // Scripts/Debug/TileEditor.cs
    public void ShowFogOfWar()
    {
        foreach (var cell in hexGrid.cells.Values)
        {
            var fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog) fog.gameObject.SetActive(true);
        }
    }

    public void HideFogOfWar()
    {
        foreach (var cell in hexGrid.cells.Values)
        {
            var fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog) fog.gameObject.SetActive(false);
        }
    }
}
