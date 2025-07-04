using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GridMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float gridSnapSize = 1f;
    public KeyCode placeKey = KeyCode.E;
    public float moveAnimationDuration = 0.2f; // Smooth movement time

    [Header("Grid Settings")]
    public string gridTag = "GroundGridSystem";
    public LayerMask obstacleLayer;

    private Vector3 targetPosition;
    private bool isPlaced = false;
    private GroundGridSystem gridSystem;
    private Rigidbody rb;
    private bool isMoving = false;
    private float moveStartTime;
    private Vector3 moveStartPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = SnapToGrid(transform.position);
        transform.position = targetPosition; // Snap immediately at start

        // Find grid system
        GameObject gridObject = GameObject.FindGameObjectWithTag(gridTag);
        if (gridObject != null)
        {
            gridSystem = gridObject.GetComponent<GroundGridSystem>();
            if (gridSystem != null)
            {
                gridSnapSize = gridSystem.cellSize;
            }
        }
    }

    void Update()
    {
        if (isPlaced) return;

        if (!isMoving)
        {
            HandleMovementInput();

            if (Input.GetKeyDown(placeKey))
            {
                PlaceObject();
            }
        }
        else
        {
            AnimateMovement();
        }
    }

    void HandleMovementInput()
    {
        // Only allow one axis movement at a time (no diagonals)
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(Vector3.right);
        }
    }

    void TryMove(Vector3 direction)
    {
        Vector3 newTarget = targetPosition + direction * gridSnapSize;
        if (CanMoveTo(newTarget))
        {
            targetPosition = newTarget;
            isMoving = true;
            moveStartTime = Time.time;
            moveStartPosition = transform.position;
        }
    }

    void AnimateMovement()
    {
        float progress = (Time.time - moveStartTime) / moveAnimationDuration;

        if (progress >= 1f)
        {
            // Movement complete
            transform.position = targetPosition;
            isMoving = false;
        }
        else
        {
            // Smooth movement between grid cells
            transform.position = Vector3.Lerp(moveStartPosition, targetPosition, progress);
        }
    }

    void PlaceObject()
    {
        isPlaced = true;
        transform.position = targetPosition;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
        enabled = false;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSnapSize) * gridSnapSize,
            position.y,
            Mathf.Round(position.z / gridSnapSize) * gridSnapSize
        );
    }

    bool CanMoveTo(Vector3 position)
    {
        // Check for obstacles
        if (Physics.CheckSphere(position, 0.4f, obstacleLayer))
        {
            return false;
        }

        // Check grid bounds
        if (gridSystem != null)
        {
            float halfGridSize = gridSystem.gridSize / 2f;
            if (Mathf.Abs(position.x) > halfGridSize || Mathf.Abs(position.z) > halfGridSize)
            {
                return false;
            }
        }

        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isPlaced ? Color.red : Color.green;
        Gizmos.DrawWireCube(targetPosition, Vector3.one * 0.9f);
    }
}