// Scripts/ScriptableObjects/HexTileData.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexTileData", menuName = "Terrain/Hex Tile Data")]
public class HexTileData : ScriptableObject
{
    [Header("Basic Info")]
    public int Id;                      // Unique identifier for this tile
    public string TerrainName;          // Name of the terrain type
    [TextArea] public string Description; // Description of the terrain type
    public GameObject Prefab;           // Prefab reference for this terrain type

    [Header("Gameplay Attributes")]
    public bool isWalkable = true;      // Determines if units can move over this tile
    public float movementCost = 1f;     // Cost of moving across this tile
    public float elevation = 0f;        // Height or elevation of this tile
    public float moisture = 0f;         //Moisture on this tile
    public float temperature = 20f;     // Temperature of the tile area
    public int population = 0;          // Population residing on this tile
    public float happiness = 1f;        // Happiness level of the tile's population
    public bool hasHousing = false;     // Determines if housing structures are present
    public bool hasVegetation = false;  // Indicates if vegetation (e.g., trees) is present
    public bool hasRoads = false;       // Determines if roads are present on this tile
    public bool hasRivers = false;      // Indicates if rivers are present on this tile
    public bool hasResources = false;   // Presence of collectible resources on the tile
    public float tileDurability = 100f; // Durability or "health" of the tile
    public bool isCorrupted = false;    // Determines if the tile is corrupted or "tainted"

    [Header("Visual Variations")]
    public List<Color> ColorVariations; // List of possible colors for the tile variation
    public List<Texture> TextureVariations; // List of possible textures for the tile variation
}
