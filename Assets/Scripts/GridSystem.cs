using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class GridSystem : MonoBehaviour
{

    [Header("Grid Settings")]
    public float gridSize = 10f;          // Total size of the grid
    public float cellSize = 1f;           // Size of each grid cell
    public Color gridColor = Color.white; // Color of the grid lines
    public float gridHeight = 0.02f;      // Height above ground for the grid

    [Header("Ground Detection")]
    public LayerMask groundLayer;         // Layer mask for ground detection
    public float maxRayDistance = 100f;   // Max distance for ground detection ray

    private LineRenderer lineRenderer;
    private bool isGridVisible = true;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = gridColor;

        DetectGroundAndCreateGrid();
    }

    void Update()
    {
        // Toggle grid visibility with G key
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGridVisibility();
        }
    }

    void DetectGroundAndCreateGrid()
    {
        // Cast a ray downward to find the ground
        Ray ray = new Ray(transform.position + Vector3.up * 10f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, groundLayer))
        {
            CreateGrid(hit.point.y + gridHeight);
        }
        else
        {
            Debug.LogWarning("No ground detected! Creating grid at default height.");
            CreateGrid(gridHeight);
        }
    }

    void CreateGrid(float height)
    {
        int divisions = Mathf.RoundToInt(gridSize / cellSize);
        int vertexCount = (divisions + 1) * 4;
        Vector3[] positions = new Vector3[vertexCount];

        float halfSize = gridSize / 2f;
        int index = 0;

        // Create vertical lines
        for (int i = 0; i <= divisions; i++)
        {
            float x = -halfSize + i * cellSize;
            positions[index++] = new Vector3(x, height, -halfSize);
            positions[index++] = new Vector3(x, height, halfSize);
        }

        // Create horizontal lines
        for (int i = 0; i <= divisions; i++)
        {
            float z = -halfSize + i * cellSize;
            positions[index++] = new Vector3(-halfSize, height, z);
            positions[index++] = new Vector3(halfSize, height, z);
        }

        lineRenderer.positionCount = vertexCount;
        lineRenderer.SetPositions(positions);
    }

    public void ToggleGridVisibility()
    {
        isGridVisible = !isGridVisible;
        lineRenderer.enabled = isGridVisible;
    }
}
