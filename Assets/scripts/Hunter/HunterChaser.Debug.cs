#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public partial class HunterChaser : MonoBehaviour
{
    private EntityDebugVisualizer hunterDebugger;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(currentPatrolTarget, 0.2f);
    }

    void InitDebug()
    {
        hunterDebugger = GetComponent<EntityDebugVisualizer>();
    }

    void PushDebugRaycastData(List<Vector2> directions, List<float> distances)
    {
        hunterDebugger?.SetRaycastDebug(directions, distances, transform.position);
    }
}
#endif
