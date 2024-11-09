// Scripts/ScriptableObjects/Ability.cs
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Game/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public int actionCost;
    public Sprite icon;                 // Optional: Icon for UI button

    public virtual void ExecuteAbility(HexCell selectedCell, TurnManager turnManager)
    {
        Debug.Log($"Executing {abilityName} ability.");
    }
}
