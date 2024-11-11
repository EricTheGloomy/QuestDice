// Scripts/Helpers/HexGridHelper.cs
using UnityEngine;

public static class HexGridHelper
{
    // Reveals fog of war in a specified range around the selected cell
    public static void RevealFogOfWar(HexCell selectedCell, int revealRange, HexGrid hexGrid)
    {
        var cellsInRange = hexGrid.GetHexesInRange(selectedCell, revealRange);
        foreach (var cell in cellsInRange)
        {
            Transform fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog != null)
            {
                fog.gameObject.SetActive(false); // Disable fog of war
            }
        }
    }

    // Places an object (e.g., tree, building) on the selected hex cell, removing any existing objects in the anchor
    public static void PlaceObjectOnHex(HexCell selectedCell, GameObject objectToPlace)
    {
        Transform objectAnchor = selectedCell.VisualRepresentation?.transform.Find("Objects");
        if (objectAnchor == null)
        {
            Debug.LogWarning("ObjectAnchor not found on the hex cell's visual representation.");
            return;
        }

        // Destroy all existing child objects in the anchor
        foreach (Transform child in objectAnchor)
        {
            Object.Destroy(child.gameObject);
        }

        // Instantiate the new object at the anchor's position
        GameObject placedObject = Object.Instantiate(objectToPlace, objectAnchor);
        placedObject.transform.localPosition = Vector3.zero; // Adjust position if needed
    }

    // Changes the type of the selected hex cell and sets fog based on enableFog parameter
    public static void ChangeHexType(HexCell selectedCell, int terrainID, HexMapVisuals hexMapVisuals, bool enableFog)
    {
        if (!hexMapVisuals.tileDataDictionary.ContainsKey(terrainID))
        {
            Debug.LogError($"Terrain ID {terrainID} does not exist in tileDataDictionary.");
            return;
        }

        HexTileData newTileData = hexMapVisuals.tileDataDictionary[terrainID];

        // Replace the current visual representation with the new type
        if (selectedCell.VisualRepresentation != null)
        {
            Object.Destroy(selectedCell.VisualRepresentation);
        }
        GameObject newVisual = TileFactory.CreateTile(newTileData, selectedCell.transform.position, selectedCell.transform);
        selectedCell.VisualRepresentation = newVisual;
        selectedCell.TerrainType = newTileData;

        // Set fog status based on enableFog parameter
        Transform fog = newVisual.transform.Find("FogOfWar");
        if (fog != null)
        {
            fog.gameObject.SetActive(enableFog);
        }
    }
}
