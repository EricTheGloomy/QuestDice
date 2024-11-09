using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public Ability[] abilities;           // Array of abilities (assigned in Inspector)
    public Button[] abilityButtons;       // Buttons corresponding to abilities
    public TurnManager turnManager;       // Reference to the TurnManager

    private HexCell selectedCell;

    private void Start()
    {
        if (abilities == null || abilities.Length == 0)
        {
            Debug.LogWarning("No abilities assigned in AbilityManager.");
        }

        if (abilityButtons == null || abilityButtons.Length == 0)
        {
            Debug.LogWarning("No buttons assigned in AbilityManager.");
        }

        if (turnManager == null)
        {
            Debug.LogError("TurnManager reference is missing in AbilityManager.");
        }

        UpdateAbilityButtons();
    }

    private void Update()
    {
        // Ensure MapInteractionManager exists and has selectedCell data
        var mapInteractionManager = FindObjectOfType<MapInteractionManager>();
        if (mapInteractionManager != null && mapInteractionManager.tileEditor != null)
        {
            selectedCell = mapInteractionManager.tileEditor.selectedCell;
        }
        else
        {
            Debug.LogWarning("MapInteractionManager or its TileEditor is missing or null.");
            selectedCell = null;
        }

        UpdateAbilityButtons();
    }

    private void UpdateAbilityButtons()
    {
        if (abilityButtons == null || turnManager == null)
        {
            Debug.LogError("Ability buttons or TurnManager not assigned properly.");
            return; // Exit if essential components are missing
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilityButtons[i] == null)
            {
                Debug.LogError($"Ability button at index {i} is missing.");
                continue;
            }

            abilityButtons[i].interactable = turnManager.remainingActions >= abilities[i].actionCost;
        }
    }

    public void UseAbility(int abilityIndex)
    {
        if (abilities == null || abilityIndex < 0 || abilityIndex >= abilities.Length || abilities[abilityIndex] == null)
        {
            Debug.LogError("Invalid ability index or ability not assigned.");
            return;
        }

        if (selectedCell != null)
        {
            abilities[abilityIndex].ExecuteAbility(selectedCell, turnManager);
            UpdateAbilityButtons();
        }
        else
        {
            Debug.LogWarning("No cell selected to use the ability on.");
        }
    }
}
