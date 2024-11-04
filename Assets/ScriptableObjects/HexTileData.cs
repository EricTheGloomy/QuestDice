using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HexTileData", menuName = "Terrain/Hex Tile Data")]
public class HexTileData : ScriptableObject
{
    [Header("Basic Info")]
    public int ID;                    // Unique identifier for the terrain type
    public string terrainName;         // Name of the terrain type
    [TextArea] public string description; // Description of the terrain type
    public GameObject prefab;          // Prefab reference for this terrain type

    [Header("Gameplay Attributes")]
    public float movementCost = 1f;    // Cost of moving over this tile (used in pathfinding)
    public float elevation = 0f;       // Height or elevation of this tile
    public bool isWalkable = true;     // Whether units can traverse this tile

    [Header("Visual Variations")]
    public List<Color> colorVariations; // List of possible colors for the tile variation
    public List<Texture> textureVariations; // List of possible textures for the tile variation
}
