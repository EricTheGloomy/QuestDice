// Scripts/Managers/TurnManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public TMP_Text turnCounterText;         // UI Text to display the turn number
    public TMP_Text actionsCounterText;      // UI Text to display the number of remaining actions
    public Button endTurnButton;         // Button to end the turn

    private int currentTurn = 1;
    public int remainingActions;
    public ActionsPerTurn actionsPerTurnSO;  // ScriptableObject for actions per turn

    private void Start()
    {
        if (actionsPerTurnSO == null)
        {
            Debug.LogError("ActionsPerTurn ScriptableObject not assigned in TurnManager.");
            return;
        }

        UpdateTurnUI();
        ResetActions();
        
        // Set up the End Turn button
        endTurnButton.onClick.AddListener(EndTurn);
    }

    // Resets actions to the max number of actions per turn
    private void ResetActions()
    {
        remainingActions = actionsPerTurnSO.maxActionsPerTurn;
        UpdateActionsUI();
    }

    // Called when End Turn button is pressed
    public void EndTurn()
    {
        currentTurn++;
        ResetActions();
        UpdateTurnUI();
    }

    // Deducts action points and updates UI, returns false if insufficient actions
    public bool SpendActions(int cost)
    {
        if (remainingActions < cost)
        {
            Debug.Log("Not enough actions to perform this ability.");
            return false;
        }

        remainingActions -= cost;
        UpdateActionsUI();
        return true;
    }

    private void UpdateTurnUI()
    {
        turnCounterText.text = $"Turn: {currentTurn}";
    }

    private void UpdateActionsUI()
    {
        actionsCounterText.text = $"Actions: {remainingActions}";
    }
}
