using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour, IInteractable
{
    public GameObject _selectionMarker;

    private void Awake()
    {
        if (_selectionMarker == null)
        {
            Debug.LogWarning("Selection marker is not assigned in TileScript on " + gameObject.name);
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
}