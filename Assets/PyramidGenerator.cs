using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public float baseSize = 2.0f;
    public float height = 2.0f;

    public Vector3 pyramidCenter;
    public Vector3 pyramidRotation;
    public Material pyramidMaterial;

    public float focalLength;

    public Vector2 RotateBy(float angle, float axis1, float axis2)
    {
        var firstAxis = axis1 * Mathf.Cos(angle) - axis2 * Mathf.Sin(angle);
        var secondAxis = axis2 * Mathf.Cos(angle) + axis1 * Mathf.Sin(angle);
        return new Vector2(firstAxis, secondAxis);
    }

    public Vector3[] GetBaseSquare()
    {
        var halfSize = baseSize * 0.5f;

        return new[] { 
            new Vector3(pyramidCenter.x + halfSize, pyramidCenter.y + halfSize, -halfSize),
            new Vector3(pyramidCenter.x - halfSize, pyramidCenter.y + halfSize, -halfSize),
            new Vector3(pyramidCenter.x - halfSize, pyramidCenter.y - halfSize, -halfSize),
            new Vector3(pyramidCenter.x + halfSize, pyramidCenter.y - halfSize, -halfSize),
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
        if(pyramidMaterial == null)
        {
            return;
        }
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        pyramidMaterial.SetPass(0);
        var baseVectors = GetBaseSquare();
        
        //shit here
        
        Vector3 apex = new Vector3(pyramidCenter.x, pyramidCenter.y, pyramidCenter.z + height);

        // z-axis rotation for base square
        for (int i = 0; i < baseVectors.Length; i++)
        {
            var deductedVector = pyramidCenter - baseVectors[i];
            var rotatedVectors = RotateBy(pyramidRotation.z, deductedVector.x, deductedVector.y);
            baseVectors[i] = new Vector3(rotatedVectors.x, rotatedVectors.y) + pyramidCenter;
        }

        // Draw base square
        var baseScale = focalLength / ((pyramidCenter.z - height * 0.5f) + focalLength);
        for (int i = 0; i < baseVectors.Length; i++)
        {
            GL.Color(pyramidMaterial.color);
            var point1 = baseVectors[i] * baseScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = baseVectors[(i + 1) % baseVectors.Length] * baseScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }

        // Draw lines from base vertices to apex
        var apexScale = focalLength / (pyramidCenter.z + height + focalLength);
        Vector3 scaledApex = apex * apexScale;
        for (int i = 0; i < baseVectors.Length; i++)
        {
            GL.Color(pyramidMaterial.color);
            var scaledBasePoint = baseVectors[i] * baseScale;
            GL.Vertex3(scaledBasePoint.x, scaledBasePoint.y, 0);
            GL.Vertex3(scaledApex.x, scaledApex.y, 0);
        }
        
        //stop here

        GL.End();
        GL.PopMatrix();
    }
}
