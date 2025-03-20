using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public float radius = 1.0f;
    public int latitudeSegments = 18; 
    public int longitudeSegments = 36;

    public Vector3 spherePosition;
    public Vector3 sphereRotation;
    public Material sphereMaterial;
    public float focalLength;

    public Vector3[] SpherePositioning()
    {
        Vector3[] points = new Vector3[latitudeSegments * longitudeSegments];
        int index = 0;

        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            float theta = lat * Mathf.PI / (latitudeSegments - 1);
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                float phi = lon * 2 * Mathf.PI / (longitudeSegments - 1);
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                float x = radius * sinTheta * cosPhi;
                float y = radius * sinTheta * sinPhi;
                float z = radius * cosTheta;

                Vector3 point = new Vector3(x, y, z);
                points[index++] = Quaternion.Euler(sphereRotation) * point + spherePosition;
            }
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
        if (sphereMaterial == null) return;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        sphereMaterial.SetPass(0);

        Vector3[] points = SpherePositioning();

        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int nextLon = (lon + 1) % longitudeSegments;
                GL.Color(sphereMaterial.color);
                Vector3 point1 = points[lat * longitudeSegments + lon];
                Vector3 point2 = points[lat * longitudeSegments + nextLon];
                GL.Vertex3(point1.x, point1.y, point1.z);
                GL.Vertex3(point2.x, point2.y, point2.z);
            }
        }

        for (int lon = 0; lon < longitudeSegments; lon++)
        {
            for (int lat = 0; lat < latitudeSegments - 1; lat++)
            {
                int nextLat = lat + 1;
                GL.Color(sphereMaterial.color);
                Vector3 point1 = points[lat * longitudeSegments + lon];
                Vector3 point2 = points[nextLat * longitudeSegments + lon];
                GL.Vertex3(point1.x, point1.y, point1.z);
                GL.Vertex3(point2.x, point2.y, point2.z);
            }
        }

        GL.End();
        GL.PopMatrix();
    }
}
