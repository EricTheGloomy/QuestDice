// Scripts/ScriptableObjects/HexTileData.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HexTileData", menuName = "Terrain/Hex Tile Data")]
public class HexTileData : ScriptableObject
{
    [Header("Basic Info")]
    public int Id;                     // Unique identifier for the terrain type
    public string TerrainName;          // Name of the terrain type
    [TextArea] public string Description; // Description of the terrain type
    public GameObject Prefab;           // Prefab reference for this terrain type

    [Header("Gameplay Attributes")]
    public float MovementCost = 1f;     // Cost of moving over this tile
    public float Elevation = 0f;        // Height or elevation of this tile
    public bool IsWalkable = true;      // Whether units can traverse this tile

    [Header("Visual Variations")]
    public List<Color> ColorVariations; // List of possible colors for the tile variation
    public List<Texture> TextureVariations; // List of possible textures for the tile variation
}
