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
        SpawnTarget();
        SpawnHunters();
        GenerateRandomWalls();
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

    void GenerateRandomWalls() {
        int wallCount = Random.Range(minWallCount, maxWallCount + 1);
        for (int i = 0; i < wallCount; i++)
        {
            Vector2 size = new(
                Random.Range(minWallSize.x, maxWallSize.x),
                Random.Range(minWallSize.y, maxWallSize.y)
            );

            Vector2 position;
            int attempts = 0;
            do
            {
                position = new Vector2(
                    Random.Range(-arenaWidth / 2 + size.x / 2, arenaWidth / 2 - size.x / 2),
                    Random.Range(-arenaHeight / 2 + size.y / 2, arenaHeight / 2 - size.y / 2)
                );
                attempts++;
            } while (target != null && Vector2.Distance(position, target.position) < minSpawnSeparation && attempts < 100);

            CreateWall(position, size);
        }
    }

    void CreateWall(Vector2 position, Vector2 scale) {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.localScale = scale;
    }

    void SpawnTarget()
    {
        Vector2 position = new(
            Random.Range(-arenaWidth * 0.4f, -arenaWidth * 0.2f),
            Random.Range(-arenaHeight * 0.4f, -arenaHeight * 0.2f)
        );

        GameObject targetObj = Instantiate(targetPrefab, position, Quaternion.identity);
        target = targetObj.transform;
    }

    void SpawnHunters()
    {
        for (int i = 0; i < numberOfHunters; i++)
        {
            Vector2 position = new(
                Random.Range(arenaWidth * 0.2f, arenaWidth * 0.4f),
                Random.Range(arenaHeight * 0.2f, arenaHeight * 0.4f)
            );

            GameObject hunter = Instantiate(hunterPrefab, position, Quaternion.identity);
            HunterChaser chaser = hunter.GetComponent<HunterChaser>();
            if (chaser != null)
                chaser.target = target;
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
