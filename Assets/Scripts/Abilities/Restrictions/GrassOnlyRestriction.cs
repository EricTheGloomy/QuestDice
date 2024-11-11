// Scripts/Abilities/Restrictions/GrassOnlyRestriction.cs
using UnityEngine;

[CreateAssetMenu(fileName = "GrassOnlyRestriction", menuName = "Game/AbilityRestrictions/GrassOnly")]
public class GrassOnlyRestriction : AbilityRestrictionBase
{
    public override bool IsMet(HexCell targetCell)
    {
        // Temporary change: Adjust logic for future ability use conditions
        // TODO: Consider new conditions or custom field for ability restrictions
        return targetCell.TerrainType.TerrainName == "Grassland";  // Placeholder terrain type
    }

    public override string GetRestrictionMessage()
    {
        return "Ability can only be used on grassland tiles.";
    }
}
