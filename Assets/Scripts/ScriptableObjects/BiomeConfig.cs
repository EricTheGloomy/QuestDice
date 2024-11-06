// Scripts/ScriptableObjects/BiomeConfig.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Terrain/Biome Config")]
public class BiomeConfig : ScriptableObject
{
    [System.Serializable]
    public struct TileTypeThreshold
    {
        public TileType TileType;
        public float MinThreshold;
        public float MaxThreshold;
    }

    public TileTypeThreshold[] thresholds;

    public TileType GetTileTypeForNoise(float noiseValue)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            var threshold = thresholds[i];
            
            // If it's the last threshold, include the Max value
            if (i == thresholds.Length - 1)
            {
                if (noiseValue >= threshold.MinThreshold && noiseValue <= threshold.MaxThreshold)
                    return threshold.TileType;
            }
            // For all other thresholds, treat Max as exclusive
            else
            {
                if (noiseValue >= threshold.MinThreshold && noiseValue < threshold.MaxThreshold)
                    return threshold.TileType;
            }
        }
        return TileType.Grass; // Default type if none match
    }
}
