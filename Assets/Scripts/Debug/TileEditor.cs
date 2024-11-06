// Scripts/Debug/TileEditor.cs
using UnityEngine;

public class TileEditor : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;

    private HexCell selectedCell;
    private int currentTileTypeIndex;
    private TileType[] tileTypes;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        tileTypes = (TileType[])System.Enum.GetValues(typeof(TileType));
        currentTileTypeIndex = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to select tile
        {
            SelectTile();
        }

        if (selectedCell != null && Input.GetKeyDown(KeyCode.Space)) // Space to cycle tile type
        {
            CycleTileType();
        }
    }

    private void SelectTile()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            selectedCell = hit.transform.GetComponentInParent<HexCell>();

            if (selectedCell != null)
            {
                Debug.Log($"Selected tile at {selectedCell.OffsetCoordinates}, position: {selectedCell.transform.position}");
                currentTileTypeIndex = System.Array.IndexOf(tileTypes, selectedCell.TerrainType.Type);
            }
        }
    }

    private void CycleTileType()
    {
        // Move to the next tile type in the array
        currentTileTypeIndex = (currentTileTypeIndex + 1) % tileTypes.Length;
        TileType newTileType = tileTypes[currentTileTypeIndex];

        Debug.Log($"Changing tile at {selectedCell.OffsetCoordinates} to {newTileType}");

        // Update the tile visuals based on the new type
        UpdateTileVisuals(selectedCell, newTileType);
    }

    private void UpdateTileVisuals(HexCell cell, TileType newTileType)
    {
        HexMapVisuals hexMapVisuals = FindObjectOfType<HexMapVisuals>();
        if (hexMapVisuals == null || !hexMapVisuals.tileDataDictionary.ContainsKey(newTileType))
        {
            Debug.LogError("TileData not found or HexMapVisuals is missing.");
            return;
        }

        HexTileData tileData = hexMapVisuals.tileDataDictionary[newTileType];
        
        // Update cell's terrain type
        cell.TerrainType = tileData;

        // Destroy current visual representation if it exists
        if (cell.VisualRepresentation != null)
        {
            DestroyImmediate(cell.VisualRepresentation); // Immediate destruction to avoid overlaps
        }

        // Create the new visual representation at the cell's position
        GameObject newTile = TileFactory.CreateTile(tileData, cell.transform.position, cell.transform);

        if (newTile != null)
        {
            // Explicitly set the new tile's local position to zero to avoid unintended offsets
            newTile.transform.localPosition = Vector3.zero;
            cell.VisualRepresentation = newTile;
            Debug.Log($"New tile created at {cell.transform.position} for tile at {cell.OffsetCoordinates}");
        }
        else
        {
            Debug.LogError("Failed to create a new visual for the tile.");
        }
    }
}
