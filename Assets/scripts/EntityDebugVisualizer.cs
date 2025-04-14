#if UNITY_EDITOR
using UnityEngine;

public abstract class EntityDebugVisualizer : MonoBehaviour {
    public static bool GlobalDebugEnabled = true;

    [Header("Debug Visuals")]
    public bool debugEnabled = true;

    public void SetDebugEnabled(bool enabled) {
        debugEnabled = enabled;
    }

    void OnDrawGizmos() {
        if (debugEnabled && GlobalDebugEnabled) {
            DrawGizmosDebug();
        }
    }

    protected abstract void DrawGizmosDebug();
}
#endif
