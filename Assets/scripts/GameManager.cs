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
    void Start()
    { 
        for (int i = 0; i < numberOfHunters; i++) {
            Vector2 spawnPos = (Vector2)target.transform.position + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject hunter = Instantiate(hunterPrefab, spawnPos, Quaternion.identity);

            HunterChaser hc = hunter.GetComponent<HunterChaser>();
            if (hc != null)
                hc.target = target.transform;
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
