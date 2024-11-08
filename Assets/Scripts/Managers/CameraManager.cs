// Scripts/Managers/CameraManager.cs
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform;        // Assign Main Camera transform in the Inspector
    public Transform cameraTarget;           // Assign CameraTarget (empty GameObject) in the Inspector
    public float moveSpeed = 10f;            // Speed of camera target movement
    public float rotationSpeed = 50f;        // Speed of rotation with Q and E keys
    public float minZoom = 10f;              // Minimum zoom level
    public float maxZoom = 80f;              // Maximum zoom level
    public float zoomSpeed = 10f;            // Speed of zoom with mouse scroll
    public float startHeight = 20f;          // Initial height of the camera above the target
    public Vector3 cameraOffset = new Vector3(0, 20, -20);  // Offset of the camera relative to target

    [SerializeField] private float initialTiltAngle = 45f;  // Tilt angle for camera to look down at the target
    [SerializeField] private Vector3 mapCenter;             // Center of the map
    [SerializeField] private float mapLimitX;               // X-axis movement limit based on map size
    [SerializeField] private float mapLimitZ;               // Z-axis movement limit based on map size

    private void Start()
    {
        HexMapVisuals hexMapVisuals = FindObjectOfType<HexMapVisuals>();
        if (hexMapVisuals == null)
        {
            Debug.LogError("HexMapVisuals not found in the scene.");
            return;
        }

        // Set map boundaries and initial position for camera target
        SetMapBounds(hexMapVisuals);
        InitializeCameraTargetPosition(hexMapVisuals);
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    // Move the camera target within XZ plane
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");  // A/D or Left/Right arrows
        float vertical = Input.GetAxis("Vertical");      // W/S or Up/Down arrows

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed * Time.deltaTime;
        cameraTarget.position += movement;

        // Constrain the target within the map boundaries
        cameraTarget.position = new Vector3(
            Mathf.Clamp(cameraTarget.position.x, mapCenter.x - mapLimitX, mapCenter.x + mapLimitX),
            cameraTarget.position.y,
            Mathf.Clamp(cameraTarget.position.z, mapCenter.z - mapLimitZ, mapCenter.z + mapLimitZ)
        );
    }

    // Rotate the camera target with Q and E keys
    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            cameraTransform.RotateAround(cameraTarget.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            cameraTransform.RotateAround(cameraTarget.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    // Zoom in/out with mouse scroll wheel
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 newOffset = cameraOffset + cameraTransform.forward * scroll * zoomSpeed;
            newOffset.y = Mathf.Clamp(newOffset.y, minZoom, maxZoom);  // Ensure zoom only affects y-axis

            cameraOffset = newOffset;  // Update offset only after clamping
        }
    }

    // Set the camera position based on target's position and offset
    private void UpdateCameraPosition()
    {
        cameraTransform.position = cameraTarget.position + cameraOffset;
        cameraTransform.LookAt(cameraTarget.position);
    }

    // Define the map boundaries based on HexGrid size
    private void SetMapBounds(HexMapVisuals hexMapVisuals)
    {
        float mapWidth = hexMapVisuals.hexGrid.mapWidth * hexMapVisuals.hexGrid.tileSizeX;
        float mapHeight = hexMapVisuals.hexGrid.mapHeight * hexMapVisuals.hexGrid.tileSizeZ;

        mapCenter = new Vector3(mapWidth / 2, 0, mapHeight / 2);
        mapLimitX = mapWidth / 2;
        mapLimitZ = mapHeight / 2;
    }

    // Position the camera target at the starting location on the map
    private void InitializeCameraTargetPosition(HexMapVisuals hexMapVisuals)
    {
        HexCell startCell = hexMapVisuals.GetStartingLocation(); // Retrieve the starting location

        if (startCell != null)
        {
            cameraTarget.position = startCell.transform.position;
            UpdateCameraPosition();
            Debug.Log($"Camera target initialized at {cameraTarget.position}");
        }
        else
        {
            Debug.LogWarning("Starting cell not found. Please verify starting location in HexGrid.");
        }
    }
}
