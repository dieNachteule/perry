using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeRenderer : MonoBehaviour
{
    public float viewAngle = 80f;
    public float viewDistance = 10f;
    public int segments = 25;

    public Color coneColor = new Color(1f, 0.5f, 0f, 0.25f); // Orange, semi-transparent

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Material coneMaterial;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Vision Cone Mesh";

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = GetComponent<MeshRenderer>();
        coneMaterial = new Material(Shader.Find("Sprites/Default"));
        coneMaterial.color = coneColor;
        meshRenderer.material = coneMaterial;
    }

    void LateUpdate()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float halfAngle = viewAngle * 0.5f;
        float angleStep = viewAngle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + angleStep * i;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 point = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * viewDistance;
            vertices[i + 1] = point;
        }

        for (int i = 0; i < segments; i++)
        {
            int triStart = i * 3;
            triangles[triStart] = 0;
            triangles[triStart + 1] = i + 1;
            triangles[triStart + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
