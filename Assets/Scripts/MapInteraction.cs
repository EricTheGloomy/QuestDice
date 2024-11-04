using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    public Camera _camera;
    //public LayerMask interactableLayer;  // Assign in the Inspector
    private IInteractable _lastSelectedTile;

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

            //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
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