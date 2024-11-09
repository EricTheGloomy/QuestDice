// Scripts/Abilities/Restrictions/GrassOnlyRestriction.cs
using UnityEngine;

[CreateAssetMenu(fileName = "GrassOnlyRestriction", menuName = "Game/AbilityRestrictions/GrassOnly")]
public class GrassOnlyRestriction : AbilityRestrictionBase
{
    public override bool IsMet(HexCell targetCell)
    {
        return targetCell.TerrainType.Type == TileType.Grass;
    }

    public override string GetRestrictionMessage()
    {
        return "Ability can only be used on grass tiles.";
    }
}
