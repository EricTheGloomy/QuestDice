// Scripts/Managers/MapInteractionManager.cs
using UnityEngine;
using System.Collections.Generic;

public class MapInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;  
    private IInteractable lastSelectedTile;

    private HexGrid hexGrid;  
    private List<GameObject> currentHighlights = new List<GameObject>(); 

    void Start()
    {
        // Assume HexGrid is attached to the same GameObject
        hexGrid = GetComponent<HexGrid>();
        if (hexGrid == null)
        {
            Debug.LogError("HexGrid component not found on GameObject.");
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Camera not set on MapInteractionManager, assigning main camera by default.");
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ProcessTileClick();
        }
    }

    private void ProcessTileClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();

            if (lastSelectedTile != null && lastSelectedTile != interactable)
            {
                lastSelectedTile.OnDeselected();
            }

            interactable?.OnClicked();
            lastSelectedTile = interactable;
        }
    }
}
