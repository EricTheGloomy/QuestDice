// Scripts/Managers/CameraManager.cs
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform;         // Reference to Main Camera
    public Transform cameraTarget;            // Reference to CameraTarget GameObject (focus point)
    public float moveSpeed = 10f;             // Speed of moving the camera target
    public float rotationSpeed = 50f;         // Speed of rotating the camera around the target
    public float minZoom = 10f;               // Minimum zoom level (height above target)
    public float maxZoom = 80f;               // Maximum zoom level
    public float zoomSpeed = 10f;             // Zoom in/out speed
    public Vector3 cameraOffset = new Vector3(0, 20, -20);  // Offset of camera relative to target

    [SerializeField] private float initialTiltAngle = 45f;  // Starting tilt angle for the camera
    [SerializeField] private Vector3 mapCenter;             // Center of the map, calculated from HexGrid
    [SerializeField] private float mapLimitX;               // X-axis map boundary limit
    [SerializeField] private float mapLimitZ;               // Z-axis map boundary limit

    private void Start()
    {
        // Find and validate HexMapVisuals and HexGrid references
        HexMapVisuals hexMapVisuals = FindObjectOfType<HexMapVisuals>();
        if (hexMapVisuals == null)
        {
            Debug.LogError("HexMapVisuals not found in the scene.");
            return;
        }

        SetMapBounds(hexMapVisuals);          // Initialize map boundary limits
        InitializeCameraTargetPosition(hexMapVisuals);
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    // Handle WASD or arrow keys for moving the camera target based on its rotation
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");  // A/D or Left/Right arrows
        float vertical = Input.GetAxis("Vertical");      // W/S or Up/Down arrows

        // Move relative to the cameraTarget's orientation
        Vector3 direction = (cameraTarget.forward * vertical + cameraTarget.right * horizontal).normalized;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        cameraTarget.position += movement;

        // Constrain the camera target within map boundaries
        cameraTarget.position = new Vector3(
            Mathf.Clamp(cameraTarget.position.x, mapCenter.x - mapLimitX, mapCenter.x + mapLimitX),
            cameraTarget.position.y,
            Mathf.Clamp(cameraTarget.position.z, mapCenter.z - mapLimitZ, mapCenter.z + mapLimitZ)
        );
    }

    // Rotate the camera around the target using Q and E keys
    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            // Rotate both camera and target to the left
            cameraTransform.RotateAround(cameraTarget.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            cameraTarget.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            // Rotate both camera and target to the right
            cameraTransform.RotateAround(cameraTarget.position, Vector3.up, rotationSpeed * Time.deltaTime);
            cameraTarget.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    // Zoom in/out using mouse scroll
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 newOffset = cameraOffset + cameraTransform.forward * scroll * zoomSpeed;
            newOffset.y = Mathf.Clamp(newOffset.y, minZoom, maxZoom); 

            cameraOffset = newOffset;
        }
    }

    // Update the camera's position relative to the target and apply rotation
    private void UpdateCameraPosition()
    {
        cameraTransform.position = cameraTarget.position + cameraOffset;
        //cameraTransform.LookAt(cameraTarget.position); //causes problems wwith rotation at min and max zooms
    }

    // Calculate map boundaries based on HexGrid size and configuration
    private void SetMapBounds(HexMapVisuals hexMapVisuals)
    {
        float mapWidth = hexMapVisuals.hexGrid.mapWidth * hexMapVisuals.hexGrid.tileSizeX;
        float mapHeight = hexMapVisuals.hexGrid.mapHeight * hexMapVisuals.hexGrid.tileSizeZ;

        mapCenter = new Vector3(mapWidth / 2, 0, mapHeight / 2);
        mapLimitX = mapWidth / 2;
        mapLimitZ = mapHeight / 2;
    }

    // Initialize camera target position at the starting location on the map
    private void InitializeCameraTargetPosition(HexMapVisuals hexMapVisuals)
    {
        HexCell startCell = hexMapVisuals.GetStartingLocation(); 

        if (startCell != null)
        {
            cameraTarget.position = startCell.transform.position;

            // Apply the initial tilt angle to the camera's rotation
            cameraTransform.position = cameraTarget.position + cameraOffset;
            cameraTransform.rotation = Quaternion.Euler(initialTiltAngle, cameraTransform.rotation.eulerAngles.y, 0);

            Debug.Log($"Camera initialized at {cameraTransform.position} with tilt {initialTiltAngle}");
        }
        else
        {
            Debug.LogWarning("Starting cell not found. Verify starting location in HexGrid.");
        }
    }
}
