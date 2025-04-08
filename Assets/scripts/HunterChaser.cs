using UnityEngine;

public class HunterChaser : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float viewDistance = 10f;
    public float viewAngle = 80f;

    private bool canSeeTarget = false;
    private Vector2 currentDirection = Vector2.right;
    private ContactFilter2D visionFilter;
    private Collider2D selfCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        selfCollider = GetComponent<Collider2D>();

        visionFilter = new ContactFilter2D();
        visionFilter.SetLayerMask(LayerMask.GetMask("Entities", "VisionBlocker"));
        visionFilter.useTriggers = false;
    }

    // Update is called once per frame
    void Update() {
        if (target == null) return;

        Vector2 directionToTarget = (target.position - transform.position);
        float distance = directionToTarget.magnitude;

        canSeeTarget = false;

        if (distance < viewDistance) {
            
            float angle = Vector2.Angle(currentDirection, directionToTarget.normalized);
            if (angle < viewAngle / 2f) {
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
                        // Vision is blocked
                        break;
                    }
                }
            }
        }

        if (canSeeTarget) {
            currentDirection = directionToTarget.normalized;
            transform.Translate(currentDirection * speed * Time.deltaTime);
        } else {
            float rotationSpeed = 90f;
            float radians = rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            currentDirection = Quaternion.Euler(0, 0, radians * Mathf.Rad2Deg) * currentDirection;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = canSeeTarget ? Color.green : Color.red;

        Vector3 leftDir = Quaternion.Euler(0, 0, -viewAngle / 2) * currentDirection;
        Vector3 rightDir = Quaternion.Euler(0, 0, viewAngle / 2) * currentDirection;

        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance);
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
