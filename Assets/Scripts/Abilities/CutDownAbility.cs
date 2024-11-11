// Scripts/Abilities/CutDownAbility.cs
using UnityEngine;

[CreateAssetMenu(fileName = "CutDownAbility", menuName = "Game/Abilities/CutDown")]
public class CutDownAbility : Ability
{
    public int replacementTerrainID;  // Terrain ID of the replacement type, e.g., grass
    public bool enableFogOnChange = true; // Controls fog state on the new tile

    public override void ExecuteAbility(HexCell selectedCell, TurnManager turnManager)
    {
        if (!CanExecute(selectedCell, out string restrictionMessage))
        {
            Debug.LogWarning(restrictionMessage);
            return;
        }

        if (turnManager.SpendActions(actionCost))
        {
            HexMapVisuals hexMapVisuals = FindObjectOfType<HexMapVisuals>();
            if (hexMapVisuals != null)
            {
                HexGridHelper.ChangeHexType(selectedCell, replacementTerrainID, hexMapVisuals, enableFogOnChange);
                Debug.Log("Cutting down forest and changing to grass.");
            }
            else
            {
                Debug.LogError("HexMapVisuals not found in the scene.");
            }
        }
    }
}
