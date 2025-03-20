using UnityEngine;

public class RectangleGenerator : MonoBehaviour
{
    public float width = 2.0f;
    public float height = 1.0f;
    public float depth = 1.0f;

    public Vector3 rectanglePosition = Vector3.zero;
    public Vector3 rectangleRotation = Vector3.zero;
    public Material rectangleMaterial;

    public Vector3[] GetFrontSquare()
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        return new[]
        {
            new Vector3(halfWidth, halfHeight, -depth * 0.5f),
            new Vector3(-halfWidth, halfHeight, -depth * 0.5f),
            new Vector3(-halfWidth, -halfHeight, -depth * 0.5f),
            new Vector3(halfWidth, -halfHeight, -depth * 0.5f)
        };
    }

    public Vector3[] GetBackSquare()
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        return new[]
        {
            new Vector3(halfWidth, halfHeight, depth * 0.5f),
            new Vector3(-halfWidth, halfHeight, depth * 0.5f),
            new Vector3(-halfWidth, -halfHeight, depth * 0.5f),
            new Vector3(halfWidth, -halfHeight, depth * 0.5f)
        };
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
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
        if (rectangleMaterial == null) return;

        GL.PushMatrix();
        rectangleMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        // Get front and back square points
        var frontSquare = GetFrontSquare();
        var backSquare = GetBackSquare();

        // Apply rotation and position to front and back squares
        for (int i = 0; i < frontSquare.Length; i++)
        {
            frontSquare[i] = RotatePointAroundPivot(frontSquare[i], Vector3.zero, rectangleRotation);
            frontSquare[i] += rectanglePosition;

            backSquare[i] = RotatePointAroundPivot(backSquare[i], Vector3.zero, rectangleRotation);
            backSquare[i] += rectanglePosition;
        }

        // Draw front square
        for (int i = 0; i < frontSquare.Length; i++)
        {
            GL.Color(rectangleMaterial.color);
            var point1 = frontSquare[i];
            var point2 = frontSquare[(i + 1) % frontSquare.Length];

            GL.Vertex3(point1.x, point1.y, point1.z);
            GL.Vertex3(point2.x, point2.y, point2.z);
        }

        // Draw back square
        for (int i = 0; i < backSquare.Length; i++)
        {
            GL.Color(rectangleMaterial.color);
            var point1 = backSquare[i];
            var point2 = backSquare[(i + 1) % backSquare.Length];

            GL.Vertex3(point1.x, point1.y, point1.z);
            GL.Vertex3(point2.x, point2.y, point2.z);
        }

        // Draw lines connecting front and back squares
        for (int i = 0; i < frontSquare.Length; i++)
        {
            GL.Color(rectangleMaterial.color);
            GL.Vertex3(frontSquare[i].x, frontSquare[i].y, frontSquare[i].z);
            GL.Vertex3(backSquare[i].x, backSquare[i].y, backSquare[i].z);
        }

        GL.End();
        GL.PopMatrix();
    }
}
