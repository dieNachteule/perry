#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public abstract class EntityDebugVisualizer : MonoBehaviour
{
    public static bool GlobalDebugEnabled = true;

    [Header("Debug Visuals")]
    public bool debugEnabled = true;

    protected List<Vector2> directions = new();
    protected List<float> distances = new();
    protected Vector2 origin;

    public void SetRaycastDebug(List<Vector2> dirs, List<float> dists, Vector2 start)
    {
        directions.Clear();
        distances.Clear();
        origin = start;

        directions.AddRange(dirs);
        distances.AddRange(dists);
    }

    public void SetDebugEnabled(bool enabled)
    {
        debugEnabled = enabled;
    }

    void OnDrawGizmos()
    {
        if (debugEnabled && GlobalDebugEnabled)
        {
            DrawGizmosDebug();
        }
    }

    protected abstract void DrawGizmosDebug();
}
#endif
