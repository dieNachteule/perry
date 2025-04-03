using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject target;
    public GameObject hunterPrefab;
    public int numberOfHunters = 3;
    public float spawnRadius = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        Vector2 spawnPos = (Vector2)target.transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject hunter = Instantiate(hunterPrefab, spawnPos, Quaternion.identity);

        HunterChaser hc = hunter.GetComponent<HunterChaser>();
        if (hc != null)
            hc.target = target.transform;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
