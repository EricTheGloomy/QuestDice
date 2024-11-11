// Scripts/Abilities/Restrictions/ForestOnlyRestriction.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ForestOnlyRestriction", menuName = "Game/AbilityRestrictions/ForestOnly")]
public class ForestOnlyRestriction : AbilityRestrictionBase
{
    public override bool IsMet(HexCell targetCell)
    {
        // Temporary change: check for terrain with vegetation instead of specific terrain type
        // TODO: Update condition to check `hasVegetation` instead of `TileType`
        return targetCell.TerrainType.TerrainName == "Plains";  // Placeholder terrain type
    }

    public override string GetRestrictionMessage()
    {
        return "Ability can only be used on forest tiles.";
    }
}
