using UnityEngine;
using System.Collections.Generic;

public class HunterChaser : MonoBehaviour
{
    public enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    public Transform target;
    public float speed = 3f;
    public float viewDistance = 10f;
    public float viewAngle = 80f;

    public float waypointTolerance = 0.2f;
    public float patrolPauseTime = 1f;
    public int patrolPointsCount = 3;
    public float patrolRadius = 5f;

    private bool canSeeTarget = false;
    private Vector2 currentDirection = Vector2.right;

    private ContactFilter2D visionFilter;
    private Collider2D selfCollider;

    private List<Vector2> patrolPoints = new();
    private int currentPatrolIndex = 0;
    private float pauseTimer = 0f;

    private float arenaWidth;
    private float arenaHeight;

    private float fovBlockCheckCooldown = 1f;
    private float fovBlockTimer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        selfCollider = GetComponent<Collider2D>();

        visionFilter = new ContactFilter2D();
        visionFilter.SetLayerMask(LayerMask.GetMask("Entities", "VisionBlocker"));
        visionFilter.useTriggers = false;

        GeneratePatrolPoints();
    }

    // Update is called once per frame
    void Update() {
        CheckLineOfSight();

        if (canSeeTarget) {
            currentState = State.Chase;
        }
        else if (currentState == State.Chase) {
            // If we were chasing and lost sight, return to patrol
            currentState = State.Patrol;
        }

        switch (currentState) {
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
        }

        fovBlockTimer -= Time.deltaTime;

        if (currentState == State.Patrol && fovBlockTimer <= 0f && IsConeBlocked())
        {
            fovBlockTimer = fovBlockCheckCooldown;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        UpdateVisionConeRotation();
    }

    public void SetArenaBounds(float width, float height) {
        arenaWidth = width;
        arenaHeight = height;
    }

    void PatrolBehavior() {
        if (patrolPoints.Count == 0) return;

        Vector2 targetPos = patrolPoints[currentPatrolIndex];
        Vector2 dir = (targetPos - (Vector2)transform.position);

        if (dir.magnitude < waypointTolerance) {
            // Arrived at point
            if (pauseTimer <= 0f) {
                pauseTimer = patrolPauseTime;
            } else {
                pauseTimer -= Time.deltaTime;
                if (pauseTimer <= 0f) {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                }
                return;
            }
        } else {
            currentDirection = dir.normalized;
            transform.Translate(currentDirection * speed * Time.deltaTime);
        }
    }

    void ChaseBehavior() {
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position);
        currentDirection = dir.normalized;
        transform.Translate(currentDirection * speed * Time.deltaTime);
    }

    void CheckLineOfSight() {
        canSeeTarget = false;

        if (target == null) return;

        Vector2 directionToTarget = (target.position - transform.position);
        float distance = directionToTarget.magnitude;

        if (distance < viewDistance) {
            float angle = Vector2.Angle(currentDirection, directionToTarget.normalized);
            if (angle < viewAngle / 2f) {
                RaycastHit2D[] hits = new RaycastHit2D[5];
                int count = Physics2D.Raycast(transform.position, directionToTarget.normalized, visionFilter, hits, viewDistance);

                for (int i = 0; i < count; i++) {
                    var hit = hits[i];
                    if (hit.collider == selfCollider) continue;

                    if (hit.collider.CompareTag("Target")) {
                        canSeeTarget = true;
                        break;
                    }
                    else if (hit.collider.CompareTag("Wall")) {
                        break;
                    }
                }
            }
        }
    }

    void GeneratePatrolPoints() {
        patrolPoints.Clear();
        Vector2 center = transform.position;
        int maxAttempts = 100;

        for (int i = 0; i < patrolPointsCount; i++) {
            int attempts = 0;
            Vector2 point;

            do {
                point = center + Random.insideUnitCircle.normalized * patrolRadius;

                // Clamp to arena bounds (adjust if your arena center is not 0,0)
                point.x = Mathf.Clamp(point.x, -arenaWidth / 2f + 0.5f, arenaWidth / 2f - 0.5f);
                point.y = Mathf.Clamp(point.y, -arenaHeight / 2f + 0.5f, arenaHeight / 2f - 0.5f);

                attempts++;
            }
            while (Physics2D.OverlapCircle(point, 0.3f, LayerMask.GetMask("VisionBlocker")) != null && attempts < maxAttempts);

            patrolPoints.Add(point);
        }
    }

    bool IsConeBlocked() {
        int raysToCast = 5;
        int hits = 0;
        float halfAngle = viewAngle / 2f;

        for (int i = 0; i <= raysToCast; i++) {
            float angle = -halfAngle + (viewAngle / raysToCast) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * currentDirection;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, LayerMask.GetMask("VisionBlocker"));
            if (hit.collider != null) {
                hits++;
            }
        }

        float blockedRatio = hits / (float)(raysToCast + 1);
        return blockedRatio >= 0.9f;
    }

    void UpdateVisionConeRotation() {
        Transform cone = transform.Find("VisionCone");
        if (cone != null) {
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            cone.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        foreach (var pt in patrolPoints)
        {
            Gizmos.DrawSphere(pt, 0.2f);
        }
    }
}
