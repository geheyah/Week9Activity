using UnityEngine;

public class RectangleGenerator : MonoBehaviour
{
    public float width = 2.0f;
    public float height = 1.0f;
    public float depth = 1.0f;

    public Vector3 rectangleCenter;
    public Vector3 rectangleRotation;
    public Material rectangleMaterial;

    public float focalLength;

    public Vector2 RotateBy(float angle, float axis1, float axis2)
    {
        var firstAxis = axis1 * Mathf.Cos(angle) - axis2 * Mathf.Sin(angle);
        var secondAxis = axis2 * Mathf.Cos(angle) + axis1 * Mathf.Sin(angle);
        return new Vector2(firstAxis, secondAxis);
    }

    public Vector3[] GetFrontSquare()
    {
        var halfWidth = width * 0.5f;
        var halfHeight = height * 0.5f;

        return new[] { 
            new Vector3(rectangleCenter.x + halfWidth, rectangleCenter.y + halfHeight, -depth * 0.5f),
            new Vector3(rectangleCenter.x - halfWidth, rectangleCenter.y + halfHeight, -depth * 0.5f),
            new Vector3(rectangleCenter.x - halfWidth, rectangleCenter.y - halfHeight, -depth * 0.5f),
            new Vector3(rectangleCenter.x + halfWidth, rectangleCenter.y - halfHeight, -depth * 0.5f),
        };
    }

    public Vector3[] GetBackSquare()
    {
        var halfWidth = width * 0.5f;
        var halfHeight = height * 0.5f;

        return new[] {
            new Vector3(rectangleCenter.x + halfWidth, rectangleCenter.y + halfHeight, depth * 0.5f),
            new Vector3(rectangleCenter.x - halfWidth, rectangleCenter.y + halfHeight, depth * 0.5f),
            new Vector3(rectangleCenter.x - halfWidth, rectangleCenter.y - halfHeight, depth * 0.5f),
            new Vector3(rectangleCenter.x + halfWidth, rectangleCenter.y - halfHeight, depth * 0.5f),
        };
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
        if(rectangleMaterial == null)
        {
            return;
        }
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        rectangleMaterial.SetPass(0);
        
        //shit here
        
        var squareVectors = GetFrontSquare();
        var backsquareVectors = GetBackSquare();

        for (int i = 0; i < squareVectors.Length; i++)
        {
            var deductedVector = rectangleCenter - squareVectors[i];
            var rotatedVectors = RotateBy(rectangleRotation.z, deductedVector.x, deductedVector.y);
            squareVectors[i] = new Vector3(rotatedVectors.x, rotatedVectors.y) + rectangleCenter;
        }

        // z-axis back square
        for (int i = 0; i < backsquareVectors.Length; i++)
        {
            var deductedVector = rectangleCenter - backsquareVectors[i];
            var rotatedVectors = RotateBy(rectangleRotation.z, deductedVector.x, deductedVector.y);
            backsquareVectors[i] = new Vector3(rotatedVectors.x, rotatedVectors.y) + rectangleCenter;
        }

        var frontScale = focalLength / ((rectangleCenter.z - depth * 0.5f) + focalLength);
        for (int i = 0; i < squareVectors.Length; i++) 
        {
            GL.Color(rectangleMaterial.color);
            var point1 = squareVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = squareVectors[(i + 1) % squareVectors.Length] * frontScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }

        var backScale = focalLength / ((rectangleCenter.z + depth * 0.5f) + focalLength);
        for (int i = 0; i < backsquareVectors.Length; i++)
        {
            GL.Color(rectangleMaterial.color);
            var point1 = backsquareVectors[i] * backScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = backsquareVectors[(i + 1) % squareVectors.Length] * backScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }

        for (int i = 0; i < backsquareVectors.Length; i++)
        {
            GL.Color(rectangleMaterial.color);
            var point1 = squareVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = backsquareVectors[i] * backScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }
        
        //stop here

        GL.End();
        GL.PopMatrix();
    }
}
