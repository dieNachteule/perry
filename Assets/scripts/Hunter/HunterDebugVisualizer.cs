#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public class HunterDebugVisualizer : EntityDebugVisualizer
{
    [SerializeField] private Color rayColor = Color.green;
    [SerializeField] private Color patrolPathColor = Color.cyan;
    [SerializeField] private Color idleColor = Color.yellow;
    [SerializeField] private Color chaseColor = Color.red;

    [SerializeField] private bool showPatrolPath = true;
    [SerializeField] private bool showStateLabel = true;

    private HunterChaser hunter;

    void Awake()
    {
        hunter = GetComponent<HunterChaser>();
    }

    protected override void DrawGizmosDebug()
    {
        if (directions == null || distances == null || directions.Count != distances.Count)
            return;

        // Draw vision rays
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2 dir = directions[i];
            float dist = distances[i];
            Gizmos.color = Color.Lerp(rayColor, Color.red, dist / 10f);
            Gizmos.DrawLine(origin, origin + dir.normalized * dist);
        }

        // Draw patrol path to current target
        if (showPatrolPath && hunter != null)
        {
            Gizmos.color = patrolPathColor;
            Gizmos.DrawLine(hunter.transform.position, hunter.GetCurrentPatrolTarget());
            Gizmos.DrawSphere(hunter.GetCurrentPatrolTarget(), 0.15f);
        }

        // Draw current state label
        if (showStateLabel && hunter != null)
        {
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(hunter.transform.position + Vector3.up * 0.5f, hunter.GetCurrentState().ToString());
        }
    }
}
#endif
