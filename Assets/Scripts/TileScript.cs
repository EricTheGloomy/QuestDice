using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour, IInteractable
{
    public GameObject _selectionMarker;

    private void Awake()
    {
        // Check if a selection marker is assigned, otherwise create a default one
        if (_selectionMarker == null)
        {
            Debug.LogWarning("Selection marker is not assigned in TileScript on " + gameObject.name + ". Creating a default marker.");
            CreateDefaultSelectionMarker();
        }
    }

    public void OnClicked()
    {
        Debug.Log("Clicked tile: " + gameObject.name);
        SetSelected(true);
    }

    public void OnDeselected()
    {
        SetSelected(false);
    }

    private void SetSelected(bool newSelected)
    {
        if (_selectionMarker != null)
        {
            _selectionMarker.SetActive(newSelected);
        }
    }

    // Method to create a default selection marker if none is assigned
    private void CreateDefaultSelectionMarker()
    {
        _selectionMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);  // Use a cube as a placeholder
        _selectionMarker.transform.SetParent(transform);                    // Make it a child of the tile
        _selectionMarker.transform.localPosition = Vector3.up * 0.5f;       // Position it slightly above the tile
        _selectionMarker.transform.localScale = new Vector3(1, 0.1f, 1);    // Scale it down to make it look like a marker
        _selectionMarker.SetActive(false);                                  // Initially set it to inactive
    }
}
