// Scripts/ScriptableObjects/Ability.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Ability", menuName = "Game/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public int actionCost;
    public Sprite icon; // Optional: Icon for UI button

    [SerializeField] private List<AbilityRestrictionBase> restrictions;  // List of restrictions

    // Checks if all restrictions are met for a given cell
    public bool CanExecute(HexCell targetCell, out string restrictionMessage)
    {
        if (restrictions == null || restrictions.Count == 0)
        {
            restrictionMessage = "";
            return true;
        }

        foreach (var restriction in restrictions)
        {
            if (!restriction.IsMet(targetCell))
            {
                restrictionMessage = restriction.GetRestrictionMessage();
                return false;
            }
        }
        restrictionMessage = "";
        return true;
    }

    public virtual void ExecuteAbility(HexCell selectedCell, TurnManager turnManager)
    {
        Debug.Log($"Executing {abilityName} ability.");
    }
}
