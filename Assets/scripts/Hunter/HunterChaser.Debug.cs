#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public partial class HunterChaser : MonoBehaviour
{
    private HunterDebugVisualizer hunterDebugger;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(currentPatrolTarget, 0.2f);
    }

    void InitDebug()
    {
        hunterDebugger = GetComponent<HunterDebugVisualizer>();
    }

    void PushDebugRaycastData(List<Vector2> directions, List<float> distances)
    {
        if (!hunterDebugger) {
            Debug.Log("hunterDebugger is null");
        }
        
        hunterDebugger?.SetRaycastDebug(directions, distances, transform.position);
    }

    public Vector2 GetCurrentPatrolTarget() => currentPatrolTarget;
    public State GetCurrentState() => currentState;
}
#endif