#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class DebugToggleMenu
{
    static DebugToggleMenu()
    {
        EditorApplication.update += () =>
        {
            if (Application.isPlaying)
            {
                // Optional: always enable by default
                EntityDebugVisualizer.GlobalDebugEnabled = true;
            }
        };
    }

    [MenuItem("Debug/Toggle Global Gizmos %#g")] // Ctrl/Cmd + Shift + G
    public static void ToggleGizmos()
    {
        EntityDebugVisualizer.GlobalDebugEnabled = !EntityDebugVisualizer.GlobalDebugEnabled;
        Debug.Log($"Global Debug Gizmos: {EntityDebugVisualizer.GlobalDebugEnabled}");
    }
}
#endif