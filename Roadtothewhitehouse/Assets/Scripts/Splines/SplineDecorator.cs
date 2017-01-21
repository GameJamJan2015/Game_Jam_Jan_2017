using System.Collections.Generic;
using UnityEngine;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

    public float distance = 0.4f;

    public float thickness = 0.5f;

    public float width = 0.5f;

    private float currentStep;

    private bool start = true;

    Mesh mesh;

    List<Vector3> newVertices = new List<Vector3>();
    List<int> newTriangles = new List<int>();
    List<Vector3> indexPostition = new List<Vector3>();

    public void RemoveAll()
    {

        mesh = GetComponent<MeshFilter>().sharedMesh;

        newTriangles.Clear();
        newVertices.Clear();

        if (mesh != null)
        {
            mesh.Clear();
            GetComponent<MeshFilter>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = null;
        }

        start = true;
    }

	public void GenerateCurve () {

        RemoveAll();
        mesh = new Mesh();
        mesh.name = "CUSTOM SPLINE";

        float stepSize = 0.0005f;
        currentStep = 0;
        while (currentStep < 1f)
        {
            currentStep += stepSize;
            Vector3 position = spline.GetPoint(currentStep);
            Vector3 direction = spline.GetDirection(currentStep);
            if (!start)
            {
                Vector3 cross = Vector3.Cross(spline.GetDirection(currentStep), Vector3.up).normalized;
                if (Vector3.Distance(newVertices[newVertices.Count - 3], position + (cross * -width)) < distance)
                    continue;
            }
            else
            {
                CreateTriangle(true, position, direction);
                start = false;
                continue;
            }

            CreateTriangle(false, position, direction);
        }
        CreateTriangle(false, spline.GetPoint(stepSize), spline.GetDirection(stepSize));

        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public Vector3 GetCenterFromVertexIndex(int index)
    {
        return indexPostition[index];
    }

    private void CreateTriangle(bool first, Vector3 currentPos, Vector3 direction)
    {
        Vector3 cross = Vector3.Cross(direction, Vector3.up).normalized;
        if(first)
        {
            newVertices.Add(currentPos + (cross * width)); //8
            newVertices.Add(currentPos + (cross * -width)); //7

            newVertices.Add(currentPos + (cross * width) + (Vector3.down * thickness)); //6
            newVertices.Add(currentPos + (cross * -width) + (Vector3.down * thickness)); //5
        }
        else
        {
            newVertices.Add(currentPos + (cross * width)); //4
            newVertices.Add(currentPos + (cross * -width)); //3

            newVertices.Add(currentPos + (cross * width) + (Vector3.down * thickness)); // 2
            newVertices.Add(currentPos + (cross * -width) + (Vector3.down * thickness)); // 1

            //top1
            newTriangles.Add(newVertices.Count - 3);
            newTriangles.Add(newVertices.Count - 7);
            newTriangles.Add(newVertices.Count - 8);

            //top2
            newTriangles.Add(newVertices.Count - 4);
            newTriangles.Add(newVertices.Count - 3);
            newTriangles.Add(newVertices.Count - 8);

            //side1
            newTriangles.Add(newVertices.Count - 1);
            newTriangles.Add(newVertices.Count - 5);
            newTriangles.Add(newVertices.Count - 3);

            //side2
            newTriangles.Add(newVertices.Count - 7);
            newTriangles.Add(newVertices.Count - 3);
            newTriangles.Add(newVertices.Count - 5);

            //other1
            newTriangles.Add(newVertices.Count - 2);
            newTriangles.Add(newVertices.Count - 4);
            newTriangles.Add(newVertices.Count - 6);

            //other2
            newTriangles.Add(newVertices.Count - 8);
            newTriangles.Add(newVertices.Count - 6);
            newTriangles.Add(newVertices.Count - 4);

            //down1
            newTriangles.Add(newVertices.Count - 1);
            newTriangles.Add(newVertices.Count - 6);
            newTriangles.Add(newVertices.Count - 5);

            //down2
            newTriangles.Add(newVertices.Count - 2);
            newTriangles.Add(newVertices.Count - 6);
            newTriangles.Add(newVertices.Count - 1);
        }

        for(int i = 0; i < 4; i++)
        {
            indexPostition.Add(currentPos);
        }
    }
}