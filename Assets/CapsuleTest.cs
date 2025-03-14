using UnityEngine;

public class CapsuleDrawer : MonoBehaviour
{
    public float width = 2.0f;
    public float height = 4.0f;
    public Material capsuleMaterial;
    
    public int segments = 36;
   
    public Vector3 position; 
    public Vector3 rotation;

    private void OnDrawGizmos()
    {
        if (capsuleMaterial == null)
        {
            return;
        }

        GL.PushMatrix();
        GL.MultMatrix(Matrix4x4.TRS(position, Quaternion.Euler(rotation), Vector3.one));
        capsuleMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        GL.Color(capsuleMaterial.color);
        GL.Vertex3(-halfWidth, halfHeight, 0);
        GL.Vertex3(halfWidth, halfHeight, 0);
        GL.Vertex3(halfWidth, -halfHeight, 0);
        GL.Vertex3(-halfWidth, -halfHeight, 0);

        for (int i = 0; i < segments / 2; i++)
        {
            float angle1 = Mathf.PI * i / (segments / 2);
            float angle2 = Mathf.PI * (i + 1) / (segments / 2);
            float x1 = halfWidth * Mathf.Cos(angle1);
            float y1 = halfHeight + halfWidth * Mathf.Sin(angle1);
            float x2 = halfWidth * Mathf.Cos(angle2);
            float y2 = halfHeight + halfWidth * Mathf.Sin(angle2);

            GL.Vertex3(x1, y1, 0);
            GL.Vertex3(x2, y2, 0);
            GL.Vertex3(-x1, y1, 0);
            GL.Vertex3(-x2, y2, 0);
        }

        for (int i = 0; i < segments / 2; i++)
        {
            float angle1 = -Mathf.PI * i / (segments / 2);
            float angle2 = -Mathf.PI * (i + 1) / (segments / 2);
            float x1 = halfWidth * Mathf.Cos(angle1);
            float y1 = -halfHeight + halfWidth * Mathf.Sin(angle1);
            float x2 = halfWidth * Mathf.Cos(angle2);
            float y2 = -halfHeight + halfWidth * Mathf.Sin(angle2);

            GL.Vertex3(x1, y1, 0);
            GL.Vertex3(x2, y2, 0);
            GL.Vertex3(-x1, y1, 0);
            GL.Vertex3(-x2, y2, 0);
        }

        GL.End();
        GL.PopMatrix();
    }
    //AYAW GUMANA
}
