using UnityEngine;

public class CylinderGenerator : MonoBehaviour
{
    public float radius = 1.0f;
    public float height = 2.0f;
    public int segments = 36;
    public Vector3 cylinderCenter;
    public Vector3 cylinderRotation;
    public Material cylinderMaterial;
    public float focalLength;

    public Vector2 RotateBy(float angle, float axis1, float axis2)
    {
        var firstAxis = axis1 * Mathf.Cos(angle) - axis2 * Mathf.Sin(angle);
        var secondAxis = axis2 * Mathf.Cos(angle) + axis1 * Mathf.Sin(angle);
        return new Vector2(firstAxis, secondAxis);
    }

    public Vector3[] GetCirclePoints(float yOffset, float scale)
    {
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = radius * scale * Mathf.Cos(angle);
            float z = radius * scale * Mathf.Sin(angle);
            points[i] = new Vector3(cylinderCenter.x + x, cylinderCenter.y + yOffset, cylinderCenter.z + z);
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
        if (cylinderMaterial == null)
        {
            return;
        }
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        cylinderMaterial.SetPass(0);
        
        //insert shit here

        float topScale = focalLength / ((cylinderCenter.z + height * 0.5f) + focalLength);
        float bottomScale = focalLength / ((cylinderCenter.z - height * 0.5f) + focalLength);

        var topCircle = GetCirclePoints(height * 0.5f, topScale);
        var bottomCircle = GetCirclePoints(-height * 0.5f, bottomScale);

        for (int i = 0; i < segments; i++)
        {
            var topRotated = RotateBy(cylinderRotation.y, topCircle[i].x, topCircle[i].z);
            topCircle[i] = new Vector3(topRotated.x, topCircle[i].y, topRotated.y);

            var bottomRotated = RotateBy(cylinderRotation.y, bottomCircle[i].x, bottomCircle[i].z);
            bottomCircle[i] = new Vector3(bottomRotated.x, bottomRotated.y, bottomRotated.y);
        }

        // Draw front circle
        for (int i = 0; i < segments; i++)
        {
            GL.Color(cylinderMaterial.color);
            var point1 = topCircle[i];
            var point2 = topCircle[(i + 1) % segments];
            GL.Vertex3(point1.x, point1.y, 0);
            GL.Vertex3(point2.x, point2.y, 0);
        }

        // Draw bottom circle
        for (int i = 0; i < segments; i++)
        {
            GL.Color(cylinderMaterial.color);
            var point1 = bottomCircle[i];
            var point2 = bottomCircle[(i + 1) % segments];
            GL.Vertex3(point1.x, point1.y, 0);
            GL.Vertex3(point2.x, point2.y, 0);
        }

        for (int i = 0; i < segments; i++)
        {
            GL.Color(cylinderMaterial.color);
            var point1 = topCircle[i];
            var point2 = bottomCircle[i];
            GL.Vertex3(point1.x, point1.y, 0);
            GL.Vertex3(point2.x, point2.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }
}
