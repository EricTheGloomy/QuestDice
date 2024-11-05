using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    public Camera _camera;
    public LayerMask interactableLayer;  // Layer for interactable objects
    private IInteractable _lastSelectedTile;

    private HexGrid hexGrid;  // Reference to the HexGrid script
    private List<GameObject> currentHighlights = new List<GameObject>(); // Store current highlights to clear later

    void Start()
    {
        // Assume HexGrid is attached to the same GameObject
        hexGrid = GetComponent<HexGrid>();
        if (hexGrid == null)
        {
            Debug.LogError("HexGrid component not found on GameObject.");
        }
    }

    void Update()
    {
        CheckClick();
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Input.mousePosition;

            // Cast a ray from the camera to the mouse position
            Ray ray = _camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();

                // Deselect the last selected tile if a new one is clicked
                if (_lastSelectedTile != null && _lastSelectedTile != interactable)
                {
                    _lastSelectedTile.OnDeselected();
                }

                interactable?.OnClicked();
                _lastSelectedTile = interactable;
            }
        }
    }
}
