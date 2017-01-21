using System.Collections.Generic;
using UnityEngine;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

	public Transform splineObject;

    public float distance = 0.4f;

    private float currentStep;

    private bool start = true;

    Mesh mesh;

    List<Vector3> newVertices = new List<Vector3>();
    List<Vector2> newUV = new List<Vector2>();
    List<int> newTriangles = new List<int>();

    public void RemoveAll()
    {

        mesh = GetComponent<MeshFilter>().sharedMesh;

        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();

        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();

        start = true;

        //objectList.Clear();
    }

	public void GenerateCurve () {
		if (splineObject == null) {
			return;
		}
        mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();

        float stepSize = 0.0005f;
        currentStep = 0;
        while (currentStep < 1f)
        {
            currentStep += stepSize;
            if (!start)
            {
                Vector3 cross = Vector3.Cross(spline.GetDirection(currentStep), Vector3.up).normalized;
                if (Vector3.Distance(newVertices[newVertices.Count - 1], spline.GetPoint(currentStep) + (cross * -2f)) < distance)
                    continue;
            }
            else
            {
                createTriangle(true, spline.GetPoint(currentStep), spline.GetDirection(currentStep));
                start = false;
                continue;
            }

            Vector3 position = spline.GetPoint(currentStep);
            Vector3 direction = spline.GetDirection(currentStep);
            createTriangle(false, position, direction);
        }

        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void createTriangle(bool first, Vector3 currentPos, Vector3 direction)
    {
        Vector3 cross = Vector3.Cross(direction, Vector3.up).normalized;
        if(first)
        {
            newVertices.Add(currentPos + (cross * 2f));
            newVertices.Add(currentPos + (cross * -2f));
        }
        else
        {
            newVertices.Add(currentPos + (cross * 2f));
            newVertices.Add(currentPos + (cross * -2f));

            newTriangles.Add(newVertices.Count - 1);
            newTriangles.Add(newVertices.Count - 3);
            newTriangles.Add(newVertices.Count - 4);

            newTriangles.Add(newVertices.Count - 2);
            newTriangles.Add(newVertices.Count - 1);
            newTriangles.Add(newVertices.Count - 4);
        }
    }
}