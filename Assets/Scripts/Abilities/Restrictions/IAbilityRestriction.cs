// Scripts/Abilities/Restrictions/IAbilityRestriction.cs
using UnityEngine;

public interface IAbilityRestriction
{
    bool IsMet(HexCell targetCell);
    string GetRestrictionMessage();  // Optional: for providing feedback to the player
}
