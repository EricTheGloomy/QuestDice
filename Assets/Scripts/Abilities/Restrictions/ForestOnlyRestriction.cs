// Scripts/Abilities/Restrictions/ForestOnlyRestriction.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ForestOnlyRestriction", menuName = "Game/AbilityRestrictions/ForestOnly")]
public class ForestOnlyRestriction : AbilityRestrictionBase
{
    public override bool IsMet(HexCell targetCell)
    {
        return targetCell.TerrainType.Type == TileType.Forest;
    }

    public override string GetRestrictionMessage()
    {
        return "Ability can only be used on forest tiles.";
    }
}
