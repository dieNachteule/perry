using UnityEngine;
using System.Collections.Generic;

public partial class HunterChaser : MonoBehaviour
{
    public enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    public Transform player;
    public float speed = 3f;
    public float viewDistance = 10f;
    public float viewAngle = 80f;

    public float waypointTolerance = 0.2f;
    public float patrolPauseTime = 1f;
    public float patrolRadius = 5f;

    private bool canSeeTarget = false;
    private Vector2 currentDirection = Vector2.right;

    private ContactFilter2D visionFilter;
    private Collider2D selfCollider;

    private Vector2 currentPatrolTarget;
    private float pauseTimer = 0f;

    private float arenaWidth;
    private float arenaHeight;

    private float fovBlockCheckCooldown = 1f;
    private float fovBlockTimer = 0f;

    void Start()
    {
        selfCollider = GetComponent<Collider2D>();

        visionFilter = new ContactFilter2D();
        visionFilter.SetLayerMask(LayerMask.GetMask("Entities", "VisionBlocker"));
        visionFilter.useTriggers = false;

        PickNewPatrolTarget();
        InitDebug();
    }

    void Update()
    {
        CheckLineOfSight();

        if (canSeeTarget)
        {
            currentState = State.Chase;
        }
        else if (currentState == State.Chase)
        {
            currentState = State.Patrol;
        }

        switch (currentState)
        {
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
            PickNewPatrolTarget();
        }

        UpdateVisionConeRotation();
    }

    public void SetArenaBounds(float width, float height)
    {
        arenaWidth = width;
        arenaHeight = height;
    }

    void CheckLineOfSight()
    {
        canSeeTarget = false;

        if (player == null) return;

        Vector2 directionToTarget = (player.position - transform.position);
        float distance = directionToTarget.magnitude;

        if (distance < viewDistance)
        {
            float angle = Vector2.Angle(currentDirection, directionToTarget.normalized);
            if (angle < viewAngle / 2f)
            {
                RaycastHit2D[] hits = new RaycastHit2D[5];
                int count = Physics2D.Raycast(transform.position, directionToTarget.normalized, visionFilter, hits, viewDistance);

                for (int i = 0; i < count; i++)
                {
                    var hit = hits[i];
                    if (hit.collider == selfCollider) continue;

                    if (hit.collider.CompareTag("Target"))
                    {
                        canSeeTarget = true;
                        break;
                    }
                    else if (hit.collider.CompareTag("Wall"))
                    {
                        break;
                    }
                }
            }
        }
    }

    void PatrolBehavior()
    {
        Vector2 dir = (currentPatrolTarget - (Vector2)transform.position);

        if (dir.magnitude < waypointTolerance)
        {
            if (pauseTimer <= 0f)
            {
                pauseTimer = patrolPauseTime;
            }
            else
            {
                pauseTimer -= Time.deltaTime;
                if (pauseTimer <= 0f)
                {
                    PickNewPatrolTarget();
                }
                return;
            }
        }
        else
        {
            currentDirection = dir.normalized;
            transform.Translate(currentDirection * speed * Time.deltaTime);
        }
    }

    void ChaseBehavior()
    {
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position);
        currentDirection = dir.normalized;
        transform.Translate(currentDirection * speed * Time.deltaTime);
    }

    void PickNewPatrolTarget()
    {
        int maxAttempts = 100;
        Vector2 center = transform.position;
        Vector2 point = center;
        int attempts = 0;

        do
        {
            point = center + Random.insideUnitCircle.normalized * patrolRadius;
            point.x = Mathf.Clamp(point.x, -arenaWidth / 2f + 0.5f, arenaWidth / 2f - 0.5f);
            point.y = Mathf.Clamp(point.y, -arenaHeight / 2f + 0.5f, arenaHeight / 2f - 0.5f);
            attempts++;
        }
        while (
            Physics2D.OverlapCircle(point, 0.3f, LayerMask.GetMask("VisionBlocker")) != null
            && attempts < maxAttempts
        );

        currentPatrolTarget = point;
    }

    bool IsConeBlocked()
    {
        int raysToCast = 10;
        int hits = 0;
        float maxUnblockedDistance = 0f;
        float minPenetrationDepth = viewDistance * 0.5f;
        float halfAngle = viewAngle / 2f;

        var debugRayDirections = new List<Vector2>();
        var debugRayDistances = new List<float>();

        for (int i = 0; i <= raysToCast; i++)
        {
            float angle = -halfAngle + (viewAngle / raysToCast) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * currentDirection;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, LayerMask.GetMask("VisionBlocker"));
            debugRayDirections.Add(dir);

            if (hit.collider != null)
            {
                hits++;
                maxUnblockedDistance = Mathf.Max(maxUnblockedDistance, hit.distance);
                debugRayDistances.Add(hit.distance);
            }
            else
            {
                maxUnblockedDistance = Mathf.Max(maxUnblockedDistance, viewDistance);
                debugRayDistances.Add(viewDistance);
            }
        }

        PushDebugRaycastData(debugRayDirections, debugRayDistances);

        float blockedRatio = hits / (float)(raysToCast + 1);
        return blockedRatio >= 0.9f && maxUnblockedDistance < minPenetrationDepth;
    }

    void UpdateVisionConeRotation()
    {
        Transform cone = transform.Find("VisionCone");
        if (cone != null)
        {
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            cone.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
