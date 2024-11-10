// Scripts/Abilities/BuildAbility.cs
using UnityEngine;

[CreateAssetMenu(fileName = "BuildAbility", menuName = "Game/Abilities/Build")]
public class BuildAbility : Ability
{
    public GameObject objectToBuild;

    public override void ExecuteAbility(HexCell selectedCell, TurnManager turnManager)
    {
        if (!CanExecute(selectedCell, out string restrictionMessage))
        {
            Debug.LogWarning(restrictionMessage);
            return;
        }

        if (turnManager.SpendActions(actionCost))
        {
            HexGridHelper.PlaceObjectOnHex(selectedCell, objectToBuild);
            Debug.Log("Building on the selected hex cell.");
        }
    }
}
