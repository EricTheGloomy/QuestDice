// Scripts/Grid/HexCell.cs
using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    public HexGrid Grid;               // Reference to the HexGrid
    public HexTileData TerrainType;            // Data for this tile type
    public Vector2 OffsetCoordinates;   // Coordinates in the grid
    public Vector3 CubeCoordinates;    // Cube coordinates
    public List<HexCell> Neighbors = new List<HexCell>();
    public GameObject VisualRepresentation;    // Reference to the visual object
    
    // Expose relevant data from TerrainType for quick access
    public bool IsWalkable => TerrainType?.isWalkable ?? true;
    public float MovementCost => TerrainType?.movementCost ?? 1f;
    public float Elevation => TerrainType?.elevation ?? 0f;
    public float Moisture => TerrainType?.moisture ?? 0f;
    public float Temperature => TerrainType?.temperature ?? 20f;
    public int Population => TerrainType?.population ?? 0;
    public float Happiness => TerrainType?.happiness ?? 1f;
    public bool HasHousing => TerrainType?.hasHousing ?? false;
    public bool HasVegetation => TerrainType?.hasVegetation ?? false;
    public bool HasRoads => TerrainType?.hasRoads ?? false;
    public bool HasRivers => TerrainType?.hasRivers ?? false;
    public bool HasResources => TerrainType?.hasResources ?? false;
    public float TileDurability => TerrainType?.tileDurability ?? 100f;
    public bool IsCorrupted => TerrainType?.isCorrupted ?? false;

    // Initialize HexCell with grid and coordinates
    public void Initialize(HexGrid grid, Vector2 offsetCoords)
    {
        Grid = grid;
        OffsetCoordinates = offsetCoords;
        CubeCoordinates = HexCoordinateHelper.AxialToCube(HexCoordinateHelper.OffsetToAxial(offsetCoords));
    }

    public void AddNeighbor(HexCell neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
            neighbor.Neighbors.Add(this);
        }
    }
}
