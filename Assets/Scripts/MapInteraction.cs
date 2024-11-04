using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    public Camera _camera;
    public GameObject neighborHighlightPrefab; // Prefab for highlighting neighbors (e.g., a small cube)
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
                    ClearHighlights(); // Clear previous highlights
                }

                interactable?.OnClicked();
                _lastSelectedTile = interactable;

                // Retrieve the HexCell component from the clicked GameObject
                HexCell clickedCell = hit.transform.GetComponent<HexCell>();

                // Check if the clicked object has a HexCell component and highlight neighbors
                if (clickedCell != null)
                {
                    HighlightNeighbors(clickedCell);
                }
            }
        }
    }

    // Method to spawn cubes on all neighbors of a clicked tile
    void HighlightNeighbors(HexCell clickedCell)
    {
        ClearHighlights(); // Clear existing highlights

        foreach (HexCell neighbor in clickedCell.Neighbors)
        {
            if (neighbor != null)
            {
                Vector2 neighborCoords;
                
                // Calculate world position based on grid orientation
                if (hexGrid.useFlatTop)
                {
                    neighborCoords = hexGrid.GetFlatTopHexCoords((int)neighbor.OffsetCoordinates.x, (int)neighbor.OffsetCoordinates.y);
                }
                else
                {
                    neighborCoords = hexGrid.GetPointyTopHexCoords((int)neighbor.OffsetCoordinates.x, (int)neighbor.OffsetCoordinates.y);
                }

                Vector3 neighborPos = new Vector3(neighborCoords.x, 1.1f, neighborCoords.y); // Set Y position slightly above ground
                GameObject highlight = Instantiate(neighborHighlightPrefab, neighborPos, Quaternion.identity);
                currentHighlights.Add(highlight);
            }
        }
    }

    // Method to clear highlighted neighbors
    void ClearHighlights()
    {
        foreach (GameObject highlight in currentHighlights)
        {
            Destroy(highlight);
        }
        currentHighlights.Clear();
    }
}
