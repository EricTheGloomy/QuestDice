// Scripts/ScriptableObjects/BiomeConfig.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Terrain/Biome Config")]
public class BiomeConfig : ScriptableObject
{
    [System.Serializable]
    public struct TileTypeThreshold
    {
        public int terrainID;           // Unique terrain ID for the tile type
        public float MinThreshold;      // Minimum noise threshold for this type
        public float MaxThreshold;      // Maximum noise threshold for this type
    }

    public TileTypeThreshold[] thresholds;

    // Returns the appropriate terrainID based on the given noise value
    public int GetTerrainIDForNoise(float noiseValue)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            var threshold = thresholds[i];
            
            // If it's the last threshold, include the Max value
            if (i == thresholds.Length - 1)
            {
                if (noiseValue >= threshold.MinThreshold && noiseValue <= threshold.MaxThreshold)
                    return threshold.terrainID;
            }
            // For all other thresholds, treat Max as exclusive
            else
            {
                if (noiseValue >= threshold.MinThreshold && noiseValue < threshold.MaxThreshold)
                    return threshold.terrainID;
            }
        }
        Debug.LogWarning("No matching terrainID found for noise value. Returning default terrainID.");
        return 0; // Return a default terrainID if none match
    }
}
