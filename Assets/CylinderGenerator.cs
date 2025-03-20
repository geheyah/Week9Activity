using UnityEditor;
using UnityEngine;

public class CylinderGenerator : MonoBehaviour
{
    public float radius = 1.0f;
    public float height = 2.0f;
    public int segments = 36;

    public Vector3 cylinderPosition = Vector3.zero; 
    public Vector3 cylinderRotation = Vector3.zero;

    public Material cylinderMaterial;
    public float focalLength = 10.0f;

    public Vector3[] GetCirclePoints(float yOffset, float scale)
    {
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = radius * scale * Mathf.Cos(angle);
            float z = radius * scale * Mathf.Sin(angle);
            points[i] = new Vector3(x, yOffset, z);
        }
        return points;
    }

    private void OnPostRender()
    {
        DrawLines();
    }

    private void OnDrawGizmos()
    {
        DrawLines();
    }

    public void DrawLines()
    {
        if (cylinderMaterial == null) return;

        GL.PushMatrix();
        cylinderMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        float topScale = focalLength / ((cylinderPosition.z + height * 0.5f) + focalLength);
        float bottomScale = focalLength / ((cylinderPosition.z - height * 0.5f) + focalLength);

        var topCircle = GetCirclePoints(height * 0.5f, topScale);
        var bottomCircle = GetCirclePoints(-height * 0.5f, bottomScale);

        for (int i = 0; i < segments; i++)
        {
            topCircle[i] = Quaternion.Euler(cylinderRotation) * topCircle[i] + cylinderPosition;
            bottomCircle[i] = Quaternion.Euler(cylinderRotation) * bottomCircle[i] + cylinderPosition;
        }

        for (int i = 0; i < segments; i++)
        {
            GL.Color(cylinderMaterial.color);
            GL.Vertex(topCircle[i]);
            GL.Vertex(topCircle[(i + 1) % segments]);
        }

        for (int i = 0; i < segments; i++)
        {
            GL.Color(cylinderMaterial.color);
            GL.Vertex(bottomCircle[i]);
            GL.Vertex(bottomCircle[(i + 1) % segments]);
        }

        for (int i = 0; i < segments; i++)
        {
            GL.Color(cylinderMaterial.color);
            GL.Vertex(topCircle[i]);
            GL.Vertex(bottomCircle[i]);
        }

        GL.End();
        GL.PopMatrix();
    }
}
