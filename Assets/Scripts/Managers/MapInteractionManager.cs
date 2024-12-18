// Scripts/Managers/MapInteractionManager.cs
using UnityEngine;
using System.Collections.Generic;

public class MapInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;  
    private IInteractable lastSelectedTile;

    private HexGrid hexGrid;  
    public TileEditor tileEditor;  // Reference to TileEditor component
    private List<GameObject> currentHighlights = new List<GameObject>(); 

    void Start()
    {
        // Locate HexGrid and TileEditor components
        hexGrid = GetComponent<HexGrid>();
        tileEditor = FindObjectOfType<TileEditor>(); // Find TileEditor in the scene

        if (hexGrid == null)
        {
            Debug.LogError("HexGrid component not found on GameObject.");
        }

        if (tileEditor == null)
        {
            Debug.LogWarning("TileEditor component not found in the scene.");
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
            // Get the clicked GameObject and the associated HexCell
            GameObject clickedObject = hit.transform.gameObject;
            HexCell clickedCell = clickedObject.GetComponentInParent<HexCell>();

            // If there’s no HexCell or fog is active, prevent interaction
            if (clickedCell == null || IsFogActive(clickedCell))
            {
                Debug.Log("Cannot interact with a tile that is covered by fog of war.");
                return;
            }

            // Update selected cell in TileEditor if TileEditor is present
            if (tileEditor != null)
            {
                tileEditor.selectedCell = clickedCell;
                Debug.Log($"TileEditor selected tile updated to: {clickedCell.OffsetCoordinates}");
            }

            // Proceed with interaction if no fog is covering the tile
            IInteractable interactable = clickedObject.GetComponent<IInteractable>();

            if (lastSelectedTile != null && lastSelectedTile != interactable)
            {
                lastSelectedTile.OnDeselected();
            }

            interactable?.OnClicked();
            lastSelectedTile = interactable;
        }
    }

    // Helper function to check if fog is active on a given tile
    private bool IsFogActive(HexCell cell)
    {
        Transform fogOfWar = cell.VisualRepresentation.transform.Find("FogOfWar");
        return fogOfWar != null && fogOfWar.gameObject.activeSelf;
    }
}
