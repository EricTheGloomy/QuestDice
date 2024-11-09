// Scripts/Testing/HexInteractionTester.cs
using UnityEngine;

public class HexInteractionTester : MonoBehaviour
{
    [Header("Settings for Testing")]
    public TileType newTileType;            // Tile type to change selected cell to (e.g., forest)
    public GameObject objectToPlace;        // Prefab for placing objects (e.g., tree, building)
    public float offsetYforPrefabAnchor = 0.99f;
    public int revealRadius = 1;            // Radius around selected cell for fog reveal
    public bool enableFogOnChange = true;   // Enable or disable fog on changed tiles

    private HexGrid hexGrid;
    private HexMapVisuals hexMapVisuals;
    private MapInteractionManager mapInteractionManager; // Reference to get selected cell

    private void Start()
    {
        hexGrid = FindObjectOfType<HexGrid>();
        hexMapVisuals = FindObjectOfType<HexMapVisuals>();
        mapInteractionManager = FindObjectOfType<MapInteractionManager>();

        if (hexGrid == null || hexMapVisuals == null || mapInteractionManager == null)
        {
            Debug.LogError("HexGrid, HexMapVisuals, or MapInteractionManager not found in the scene.");
        }
    }

    private void Update()
    {
        // Get the currently selected cell from MapInteractionManager
        HexCell selectedCell = GetSelectedCell();

        // Key shortcuts for testing each function
        if (Input.GetKeyDown(KeyCode.J))
        {
            RevealFogOfWar(selectedCell, revealRadius);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlaceObjectOnHex(selectedCell);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeHexType(selectedCell, newTileType);
        }
    }

    // Get currently selected cell from MapInteractionManager
    private HexCell GetSelectedCell()
    {
        return mapInteractionManager != null && mapInteractionManager.tileEditor != null
            ? mapInteractionManager.tileEditor.selectedCell
            : null;
    }

    // Reveals the fog of war around the selected hex cell within a specified range
    public void RevealFogOfWar(HexCell selectedCell, int revealRange = 1)
    {
        if (selectedCell == null)
        {
            Debug.LogWarning("No hex cell selected.");
            return;
        }

        var cellsInRange = hexGrid.GetHexesInRange(selectedCell, revealRange);
        foreach (var cell in cellsInRange)
        {
            Transform fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog != null)
            {
                fog.gameObject.SetActive(false); // Disable fog of war
            }
        }
        Debug.Log($"Revealed fog around cell at {selectedCell.OffsetCoordinates} within range {revealRange}.");
    }

    // Places an object (e.g., tree, building) on the selected hex cell, removing any existing objects in the anchor
    public void PlaceObjectOnHex(HexCell selectedCell)
    {
        if (selectedCell == null)
        {
            Debug.LogWarning("No hex cell selected.");
            return;
        }

        Transform objectAnchor = selectedCell.VisualRepresentation?.transform.Find("Objects");
        if (objectAnchor == null)
        {
            Debug.LogWarning("ObjectAnchor not found on the hex cell's visual representation.");
            return;
        }

        // Destroy all existing child objects in the anchor
        foreach (Transform child in objectAnchor)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the new object at the correct position and offset it by 0.99 on the Y axis
        if (objectToPlace != null)
        {
            GameObject placedObject = Instantiate(objectToPlace, objectAnchor);
            placedObject.transform.localPosition = new Vector3(0, offsetYforPrefabAnchor, 0); // Set Y offset to 0.99

            Debug.Log($"Placed {objectToPlace.name} on cell at {selectedCell.OffsetCoordinates}.");
        }
        else
        {
            Debug.LogWarning("Object to place is not assigned.");
        }
    }

    // Changes the type of the selected hex cell (e.g., from grass to forest) and sets fog based on 'enableFogOnChange'
    public void ChangeHexType(HexCell selectedCell, TileType newTileType)
    {
        if (selectedCell == null)
        {
            Debug.LogWarning("No hex cell selected.");
            return;
        }

        if (!hexMapVisuals.tileDataDictionary.ContainsKey(newTileType))
        {
            Debug.LogError($"TileType {newTileType} does not exist in tileDataDictionary.");
            return;
        }

        HexTileData newTileData = hexMapVisuals.tileDataDictionary[newTileType];

        // Replace the current visual representation with the new type
        if (selectedCell.VisualRepresentation != null)
        {
            Destroy(selectedCell.VisualRepresentation);
        }
        GameObject newVisual = TileFactory.CreateTile(newTileData, selectedCell.transform.position, selectedCell.transform);
        selectedCell.VisualRepresentation = newVisual;
        selectedCell.TerrainType = newTileData;

        // Enable or disable fog based on 'enableFogOnChange'
        Transform fog = newVisual.transform.Find("FogOfWar");
        if (fog != null)
        {
            fog.gameObject.SetActive(enableFogOnChange);
        }

        Debug.Log($"Changed hex type at {selectedCell.OffsetCoordinates} to {newTileType} with fog set to {enableFogOnChange}.");
    }
}
