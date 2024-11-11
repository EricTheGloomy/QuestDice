// Scripts/Tiles/TileScript.cs
using UnityEngine;

public class TileScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject selectionMarker;  // Allows for flexibility in Unity Inspector
    [SerializeField] private GameObject fogOfWar; 
    [SerializeField] private GameObject groundObjects; 

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        // Create a default selection marker if none is assigned
        if (selectionMarker == null)
        {
            Debug.LogWarning("Selection marker not assigned in TileScript on " + gameObject.name + ". Creating default.");
            CreateDefaultSelectionMarker();
        }
    }

    public void OnClicked()
    {
        Debug.Log("Tile clicked: " + gameObject.name);
        SetSelected(true);
    }

    public void OnDeselected()
    {
        SetSelected(false);
    }

    private void SetSelected(bool isSelected)
    {
        if (selectionMarker != null)
        {
            selectionMarker.SetActive(isSelected);
        }
    }

    private void CreateDefaultSelectionMarker()
    {
        selectionMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        selectionMarker.transform.SetParent(transform);
        selectionMarker.transform.localPosition = Vector3.up * 0.5f;
        selectionMarker.transform.localScale = new Vector3(1, 0.1f, 1);
        selectionMarker.SetActive(false);  
    }
}
