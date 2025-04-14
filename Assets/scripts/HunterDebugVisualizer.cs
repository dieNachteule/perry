#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public class HunterDebugVisualizer : EntityDebugVisualizer
{
    private List<Vector2> directions = new();
    private List<float> distances = new();
    private Vector2 origin;

    public void SetRaycastDebug(List<Vector2> dirs, List<float> dists, Vector2 start)
    {
        directions = new List<Vector2>(dirs);
        distances = new List<float>(dists);
        origin = start;
    }

    protected override void DrawGizmosDebug()
    {
        if (directions == null || distances == null || directions.Count != distances.Count)
            return;

        for (int i = 0; i < directions.Count; i++)
        {
            Vector2 dir = directions[i];
            float dist = distances[i];

            Gizmos.color = Color.Lerp(Color.green, Color.red, dist / 10f);
            Gizmos.DrawLine(origin, origin + dir.normalized * dist);
        }
    }
}
#endif