// Scripts/Abilities/BuildAbility.cs
using UnityEngine;

[CreateAssetMenu(fileName = "BuildAbility", menuName = "Game/Abilities/Build")]
public class BuildAbility : Ability
{
    public GameObject objectToBuild;

    public override void ExecuteAbility(HexCell selectedCell, TurnManager turnManager)
    {
        if (turnManager.SpendActions(actionCost))
        {
            if (objectToBuild != null)
            {
                HexGridHelper.PlaceObjectOnHex(selectedCell, objectToBuild);
                Debug.Log("Building on the selected hex cell.");
            }
            else
            {
                Debug.LogWarning("Object to build is not assigned.");
            }
        }
    }
}
