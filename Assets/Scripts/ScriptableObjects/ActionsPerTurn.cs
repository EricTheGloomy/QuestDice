// Scripts/ScriptableObjects/ActionsPerTurn.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ActionsPerTurn", menuName = "Game/ActionsPerTurn")]
public class ActionsPerTurn : ScriptableObject
{
    public int maxActionsPerTurn = 5;  // Default number of actions per turn
}
