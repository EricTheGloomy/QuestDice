// Scripts/Abilities/ExploreAbility.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ExploreAbility", menuName = "Game/Abilities/Explore")]
public class ExploreAbility : Ability
{
    public int revealRadius = 2;

    public override void ExecuteAbility(HexCell selectedCell, TurnManager turnManager)
    {
        if (turnManager.SpendActions(actionCost))
        {
            HexGrid hexGrid = FindObjectOfType<HexGrid>();
            if (hexGrid != null)
            {
                HexGridHelper.RevealFogOfWar(selectedCell, revealRadius, hexGrid);
                Debug.Log("Exploring and revealing fog of war.");
            }
            else
            {
                Debug.LogError("HexGrid not found in the scene.");
            }
        }
    }
}
