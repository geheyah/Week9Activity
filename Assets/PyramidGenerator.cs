using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public float baseSize = 2.0f;
    public float height = 2.0f;

    public Vector3 pyramidPosition = Vector3.zero;
    public Vector3 pyramidRotation = Vector3.zero;
    public Material pyramidMaterial;

    public Vector3[] GetBaseSquare()
    {
        var halfSize = baseSize * 0.5f;

        return new[]
        {
            new Vector3(halfSize, 0, -halfSize),
            new Vector3(-halfSize, 0, -halfSize),
            new Vector3(-halfSize, 0, halfSize),
            new Vector3(halfSize, 0, halfSize)
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
        if (pyramidMaterial == null)
        {
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        pyramidMaterial.SetPass(0);

        // Get base square points
        var baseVectors = GetBaseSquare();
        Vector3 apex = new Vector3(0, height, 0);

        // Apply rotation and position to base and apex points
        for (int i = 0; i < baseVectors.Length; i++)
        {
            baseVectors[i] = RotatePointAroundPivot(baseVectors[i], Vector3.zero, pyramidRotation);
            baseVectors[i] += pyramidPosition;
        }

        apex = RotatePointAroundPivot(apex, Vector3.zero, pyramidRotation);
        apex += pyramidPosition;

        // Draw base square
        for (int i = 0; i < baseVectors.Length; i++)
        {
            GL.Color(pyramidMaterial.color);
            var point1 = baseVectors[i];
            var point2 = baseVectors[(i + 1) % baseVectors.Length];

            GL.Vertex3(point1.x, point1.y, point1.z);
            GL.Vertex3(point2.x, point2.y, point2.z);
        }

        // Draw lines from apex to base
        for (int i = 0; i < baseVectors.Length; i++)
        {
            GL.Color(pyramidMaterial.color);

            GL.Vertex3(baseVectors[i].x, baseVectors[i].y, baseVectors[i].z);
            GL.Vertex3(apex.x, apex.y, apex.z);
        }

        GL.End();
        GL.PopMatrix();
    }
}
