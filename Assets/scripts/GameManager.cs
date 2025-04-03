using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject targetPrefab;
    public GameObject hunterPrefab;
    public GameObject wallPrefab;

    [Header("Arena Setttings")]
    public float arenaWidth = 20f;
    public float arenaHeight = 20f;

    [Header("Wall Generation")]
    public int minWallCount = 5;
    public int maxWallCount = 10;
    public Vector2 minWallSize = new(1f, 1f);
    public Vector2 maxWallSize = new(5f, 5f);

    [Header("Entities")]
    public int numberOfHunters = 3;
    public float minSpawnSeparation = 5f;

    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { 
        GenerateBoundaryWalls();
    }

    void GenerateBoundaryWalls() {
        Vector2 center = Vector2.zero;
        float thickness = 1f;

        // Top
        CreateWall(new Vector2(center.x, arenaHeight / 2 + thickness / 2), new Vector2(arenaWidth + thickness * 2, thickness));
        // Bottom
        CreateWall(new Vector2(center.x, -arenaHeight / 2 - thickness / 2), new Vector2(arenaWidth + thickness * 2, thickness));
        // Left
        CreateWall(new Vector2(-arenaWidth / 2 - thickness / 2, center.y), new Vector2(thickness, arenaHeight));
        // Right
        CreateWall(new Vector2(arenaWidth / 2 + thickness / 2, center.y), new Vector2(thickness, arenaHeight));
    }

    void CreateWall(Vector2 position, Vector2 scale) {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.localScale = scale;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
