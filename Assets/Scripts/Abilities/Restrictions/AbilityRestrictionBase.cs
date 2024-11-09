// Scripts/Abilities/Restrictions/AbilityRestrictionBase.cs
using UnityEngine;

public abstract class AbilityRestrictionBase : ScriptableObject, IAbilityRestriction
{
    public abstract bool IsMet(HexCell targetCell);
    public abstract string GetRestrictionMessage();
}
